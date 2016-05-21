﻿using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using SuperGlue.Cms.Parsing;
using SuperGlue.Cms.Rendering;

namespace SuperGlue.Cms.Localization
{
    public class LocalizationTextParser : RegexTextParser
    {
        private readonly ILocalizeText _localizeText;
        private readonly IFindCurrentLocalizationNamespace _findCurrentLocalizationNamespace;
        private readonly CultureInfo _culture;

        public LocalizationTextParser(ILocalizeText localizeText, IFindCurrentLocalizationNamespace findCurrentLocalizationNamespace, CultureInfo culture)
        {
            _localizeText = localizeText;
            _findCurrentLocalizationNamespace = findCurrentLocalizationNamespace;
            _culture = culture;
        }

        protected override async Task<object> FindParameterValue(Match match, ICmsRenderer cmsRenderer, IDictionary<string, object> environment, IReadOnlyDictionary<string, dynamic> dataSources)
        {
            var localizationNamespace = _findCurrentLocalizationNamespace.Find();

            var key = !string.IsNullOrEmpty(localizationNamespace) ?
                $"{localizationNamespace}:{match.Groups["resource"].Value}"
                : match.Groups["resource"].Value;

            var replacements = new Dictionary<string, string>();

            var replacementsGroup = match.Groups["replacements"];

            if (!string.IsNullOrEmpty(replacementsGroup?.Value))
            {
                var replacementsData = replacementsGroup
                    .Value
                    .Split(',')
                    .Where(x => !string.IsNullOrEmpty(x))
                    .Select(x => x.Split(':'))
                    .Where(x => x.Length > 1 && !string.IsNullOrEmpty(x[0]) && !string.IsNullOrEmpty(x[1]))
                    .ToList();

                foreach (var item in replacementsData)
                    replacements[item[0]] = item[1];
            }

            var localized = await _localizeText.Localize(key, _culture).ConfigureAwait(false);

            localized = replacements.Aggregate(localized, (current, replacement) => current.Replace(string.Concat("{", replacement.Key, "}"), replacement.Value));

            return await cmsRenderer.ParseText(localized, environment, dataSources.ToDictionary(x => x.Key, x => x.Value)).ConfigureAwait(false);
        }

        protected override IEnumerable<Regex> GetRegexes()
        {
            yield return new Regex(@"\%\[(?<resource>.[a-z&auml;&aring;&ouml;A-Z&Auml;&Aring;&Ouml;0-9_\:-]*)\]\%", RegexOptions.Compiled);
            yield return new Regex(@"\%\[(?<resource>.[a-z&auml;&aring;&ouml;A-Z&Auml;&Aring;&Ouml;0-9_\:-]*) replacements\=((?<replacements>[a-z&auml;&aring;&ouml;A-Z&Auml;&Aring;&Ouml;0-9\:\,]*))\]\%", RegexOptions.Compiled);
        }
    }
}