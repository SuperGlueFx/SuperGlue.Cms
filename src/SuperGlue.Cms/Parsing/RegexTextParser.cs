using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using SuperGlue.Cms.Rendering;

namespace SuperGlue.Cms.Parsing
{
    public abstract class RegexTextParser : ITextParser
    {
        protected virtual string SeperateListItemsWith => "\n";

        public string Parse(string text, ICmsRenderer cmsRenderer, ICmsContext context, Func<string, string> recurse)
        {
            text = text ?? "";

            text = GetRegexes().Aggregate(text, (current, regex) => regex.Replace(current, x =>
            {
                var value = FindParameterValue(x, cmsRenderer, context, recurse);

                if (value == null) return "";

                var enumerableValue = value as IEnumerable;
                if (enumerableValue != null && !(value is string))
                {
                    var stringValues = enumerableValue.OfType<object>().Select(y => y.ToString()).ToList();

                    return string.Join(SeperateListItemsWith, stringValues);
                }

                return value.ToString();
            }));

            return text;
        }

        public abstract IEnumerable<string> GetTags();
        protected abstract object FindParameterValue(Match match, ICmsRenderer cmsRenderer, ICmsContext context, Func<string, string> recurse);
        protected abstract IEnumerable<Regex> GetRegexes();
    }
}