using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using SuperGlue.Cms.Rendering;

namespace SuperGlue.Cms.Parsing
{
    public class ContextParametersTextParser : RegexTextParser
    {
        private readonly IFindParameterValueFromModel _findParameterValueFromModel;

        public ContextParametersTextParser(IFindParameterValueFromModel findParameterValueFromModel)
        {
            _findParameterValueFromModel = findParameterValueFromModel;
        }

        public override IEnumerable<string> GetTags()
        {
            yield return "parameter";
            yield return "multitarget";
        }

        protected override object FindParameterValue(Match match, ICmsRenderer cmsRenderer, ICmsContext context, Func<string, string> recurse)
        {
            var contextNameGroup = match.Groups["contextName"];

            if (string.IsNullOrEmpty(contextNameGroup?.Value))
                return "";

            var path = match.Groups["contextName"].Value;

            var parts = path.Split('.');

            var contextName = parts.FirstOrDefault();

            var requestContext = context.FindContext(contextName);

            if (requestContext == null)
                return null;

            var settingsName = parts.Skip(1).FirstOrDefault();

            if (!requestContext.Data.ContainsKey(settingsName))
                return null;

            var value = _findParameterValueFromModel.Find(path.Substring($"{contextName}.{settingsName}.".Length), requestContext.Data[settingsName]);

            var stringValue = value as string;

            return stringValue != null ? recurse(stringValue) : value;
        }

        protected override IEnumerable<Regex> GetRegexes()
        {
            yield return new Regex(@"\!\[Contexts\.((?<contextName>.[a-z&auml;&aring;&ouml;A-Z&Auml;&Aring;&Ouml;0-9.""\]\[]*))\]\!", RegexOptions.Compiled);
        }
    }
}