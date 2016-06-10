using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using SuperGlue.Configuration;
using SuperGlue.FileSystem;

namespace SuperGlue.Cms.Localization
{
    public class DefaultLocalizer : ILocalizeText
    {
        private static readonly IDictionary<string, string> Translations = new ConcurrentDictionary<string, string>();
        public const string LeafElement = "string";
        private readonly IFileSystem _fileSystem;
        private readonly IEnumerable<ILocalizationVisitor> _visitors;
        private readonly IDictionary<string, object> _environment;

        public DefaultLocalizer(IEnumerable<ILocalizationVisitor> visitors, IFileSystem fileSystem, IDictionary<string, object> environment)
        {
            _visitors = visitors;
            _fileSystem = fileSystem;
            _environment = environment;
        }

        public virtual Task<string> Localize(string key, CultureInfo culture)
        {
            var translationKey = key;

            while (true)
            {
                var translationResult = GetTranslation(translationKey, culture);

                if (translationResult.Item2)
                    return Task.FromResult(_visitors.Aggregate(translationResult.Item1, (current, visitor) => visitor.AfterLocalized(translationKey, current)));

                var keyParts = translationKey.Split(':');

                var text = translationKey;

                if (keyParts.Length <= 1)
                    return Task.FromResult(text);

                var namespaceParts = keyParts.Take(keyParts.Length - 1).ToArray();

                if (namespaceParts.Length <= 0)
                    return Task.FromResult(text);

                var namespacePartsToUse = namespaceParts.Take(namespaceParts.Length - 1).ToArray();
                translationKey = keyParts.Last();

                if (namespacePartsToUse.Any())
                    translationKey = $"{string.Join(":", namespacePartsToUse)}:{translationKey}";
            }
        }

        public virtual Task Load()
        {
            var fileSet = new FileSet
            {
                DeepSearch = false,
                Include = "*.locale.config"
            };

            var directories = GetDirectoriesToSearch();

            var groups = directories.SelectMany(dir => _fileSystem.FindFiles(dir, fileSet)).Where(file =>
            {
                var fileName = Path.GetFileName(file);
                return fileName != null;
            }).GroupBy(CultureFor);

            foreach(var group in groups)
            {
                var items = group.SelectMany(LoadFrom);

                foreach (var item in items)
                {
                    var key = BuildKey(group.Key, item.Item1);

                    Translations[key] = item.Item2;
                }
            }

            return Task.CompletedTask;
        }

        protected virtual Tuple<string, bool> GetTranslation(string key, CultureInfo culture)
        {
            var translationKey = BuildKey(culture, key);

            if (Translations.ContainsKey(translationKey))
                return new Tuple<string, bool>(Translations[translationKey], true);

            return new Tuple<string, bool>("", false);
        }

        protected virtual string GetDefaultText(string key, CultureInfo culture)
        {
            return key;
        }

        protected virtual IEnumerable<string> GetDirectoriesToSearch()
        {
            yield return _environment.ResolvePath("~/");
        }

        private CultureInfo CultureFor(string filename)
        {
            return new CultureInfo(_fileSystem.GetFileName(filename).Split('.').First());
        }

        private static IEnumerable<Tuple<string, string>> LoadFrom(string file)
        {
            var document = XDocument.Load(file);

            var xmlNodeList = document.Root?.Elements(LeafElement);

            if (xmlNodeList == null)
                yield break;

            foreach (var element in xmlNodeList)
                yield return new Tuple<string, string>(element.Attribute("key").ToString(), element.Value);
        }

        private static string BuildKey(CultureInfo culture, string key)
        {
            return $"{culture.Name.ToLower()}-{key}";
        }
    }
}