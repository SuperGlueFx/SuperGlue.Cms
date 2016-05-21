using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
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

            return cmsRenderer.RenderTemplate(template);
        }

        protected override IEnumerable<Regex> GetRegexes()
        {
            yield return new Regex(@"\!\[Templates\.((?<templateName>.[a-z&auml;&aring;&ouml;A-Z&Auml;&Aring;&Ouml;0-9.\-]*))\]\!", RegexOptions.Compiled);
        }
    }
}