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
        private static IDictionary<string, string> translations;
        public const string LeafElement = "string";
        private readonly IFileSystem _fileSystem;
        private readonly IEnumerable<ILocalizationVisitor> _visitors;
        private readonly IDictionary<string, object> _environment;
        private static readonly object LoadLock = new object();

        public DefaultLocalizer(IEnumerable<ILocalizationVisitor> visitors, IFileSystem fileSystem, IDictionary<string, object> environment)
        {
            _visitors = visitors;
            _fileSystem = fileSystem;
            _environment = environment;
        }

        public virtual Task<string> Localize(string key, CultureInfo culture)
        {
            var translationKey = key;
            var writeMissing = true;
            var missingKeys = new List<string>();

            while (true)
            {
                var translationResult = GetTranslation(translationKey, culture);

                if (translationResult.Item2)
                    return Task.FromResult(_visitors.Aggregate(translationResult.Item1, (current, visitor) => visitor.AfterLocalized(translationKey, current)));

                if (writeMissing)
                    missingKeys.Add(translationKey);

                var keyParts = translationKey.Split(':');

                if (keyParts.Length <= 1)
                {
                    WriteMissing(missingKeys, culture);

                    return Task.FromResult(GetDefaultText(translationKey, culture));
                }

                var namespaceParts = keyParts.Take(keyParts.Length - 1).ToArray();

                if (namespaceParts.Length <= 0)
                {
                    WriteMissing(missingKeys, culture);

                    return Task.FromResult(GetDefaultText(translationKey, culture));
                }

                var namespacePartsToUse = namespaceParts.Take(namespaceParts.Length - 1).ToArray();
                translationKey = keyParts.Last();

                if (namespacePartsToUse.Any())
                    translationKey = $"{string.Join(":", namespacePartsToUse)}:{translationKey}";

                writeMissing = false;
            }
        }

        protected virtual void WriteMissing(IEnumerable<string> keys, CultureInfo culture)
        {
            
        }

        protected virtual void Load()
        {
            var fileSet = new FileSet
            {
                DeepSearch = false,
                Include = "*.locale.config"
            };

            var directories = GetDirectoriesToSearch();

            lock (LoadLock)
            {
                translations = new ConcurrentDictionary<string, string>();

                var groups = directories.SelectMany(dir => _fileSystem.FindFiles(dir, fileSet)).Where(file =>
                {
                    var fileName = Path.GetFileName(file);
                    return fileName != null;
                }).GroupBy(CultureFor);

                foreach (var group in groups)
                {
                    var items = group.SelectMany(LoadFrom);

                    foreach (var item in items)
                    {
                        var key = BuildKey(group.Key, item.Item1);

                        translations[key] = item.Item2;
                    }
                }
            }
        }

        protected virtual Tuple<string, bool> GetTranslation(string key, CultureInfo culture)
        {
            if(translations == null)
                Load();

            var translationKey = BuildKey(culture, key);

            if (translations?.ContainsKey(translationKey) ?? false)
                return new Tuple<string, bool>(translations[translationKey], true);

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