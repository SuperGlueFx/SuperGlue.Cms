using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
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

        public override IEnumerable<string> GetTags()
        {
            yield return "template";
            yield return "advanced";
            yield return "multitarget";
        }

        protected override object FindParameterValue(Match match, ICmsRenderer cmsRenderer, ICmsContext context, Func<string, string> recurse)
        {
            var templateNameGroup = match.Groups["templateName"];

            if (templateNameGroup == null)
                return "";

            var templateName = templateNameGroup.Value;

            var template = _templateStorage.Load(templateName);

            if (template == null)
                return "";

            var settings = new Dictionary<string, object>();

            var settingsGroup = match.Groups["settings"];

            if (!string.IsNullOrEmpty(settingsGroup?.Value))
            {
                var settingsJson = settingsGroup.Value;

                if (!string.IsNullOrEmpty(settingsJson))
                {
                    var parsedSettings = JsonConvert.DeserializeObject<IDictionary<string, object>>(settingsJson);

                    foreach (var item in parsedSettings)
                        settings[item.Key] = item.Value;
                }
            }

            var renderResult = cmsRenderer.RenderTemplate(template, settings, context);

            return renderResult.Read();
        }

        protected override IEnumerable<Regex> GetRegexes()
        {
            yield return new Regex(@"\!\[Templates\.((?<templateName>.[a-z&auml;&aring;&ouml;A-Z&Auml;&Aring;&Ouml;0-9.\-]*))\]\!", RegexOptions.Compiled);
            yield return new Regex(@"\!\[Templates\.((?<templateName>.[a-z&auml;&aring;&ouml;A-Z&Auml;&Aring;&Ouml;0-9.\-]*)) Settings\=((?<settings>[a-z&auml;&aring;&ouml;A-Z&Auml;&Aring;&Ouml;0-9.""\]\[\{\}]*))\]\!", RegexOptions.Compiled);
        }
    }
}