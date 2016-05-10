using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SuperGlue.Cms.Rendering;
using SuperGlue.Cms.Templates;

namespace SuperGlue.Cms.Parsing
{
    public class TemplateTextParser : RegexTextParser
    {
        private readonly ITemplateStorage _templateStorage;

        public TemplateTextParser(ITemplateStorage templateStorage)
        {
            _templateStorage = templateStorage;
        }

        protected override async Task<object> FindParameterValue(Match match, ICmsRenderer cmsRenderer, Func<string, Task<string>> recurse)
        {
            var templateNameGroup = match.Groups["templateName"];

            if (templateNameGroup == null)
                return "";

            var templateName = templateNameGroup.Value;

            var template = await _templateStorage.Load(templateName).ConfigureAwait(false);

            if (template == null)
                return "";

            var settings = new Dictionary<string, object>();

            var settingsGroup = match.Groups["settings"];

            if (string.IsNullOrEmpty(settingsGroup?.Value))
                return cmsRenderer.RenderTemplate(template, settings);

            var settingsJson = settingsGroup.Value;

            if (string.IsNullOrEmpty(settingsJson))
                return cmsRenderer.RenderTemplate(template, settings);

            var parsedSettings = JsonConvert.DeserializeObject<IDictionary<string, object>>(settingsJson);

            foreach (var item in parsedSettings)
                settings[item.Key] = item.Value;

            return cmsRenderer.RenderTemplate(template, settings);
        }

        protected override IEnumerable<Regex> GetRegexes()
        {
            yield return new Regex(@"\!\[Templates\.((?<templateName>.[a-z&auml;&aring;&ouml;A-Z&Auml;&Aring;&Ouml;0-9.\-]*))\]\!", RegexOptions.Compiled);
            yield return new Regex(@"\!\[Templates\.((?<templateName>.[a-z&auml;&aring;&ouml;A-Z&Auml;&Aring;&Ouml;0-9.\-]*)) Settings\=((?<settings>[a-z&auml;&aring;&ouml;A-Z&Auml;&Aring;&Ouml;0-9.""\]\[\{\}]*))\]\!", RegexOptions.Compiled);
        }
    }
}