using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using SuperGlue.Cms.Rendering;

namespace SuperGlue.Cms.Parsing
{
    public abstract class RegexTextParser : ITextParser
    {
        protected virtual string SeperateListItemsWith => "\n";

        public async Task<string> Parse(string text, ICmsRenderer cmsRenderer, Func<string, Task<string>> recurse)
        {
            text = text ?? "";

            foreach (var regex in GetRegexes())
            {
                text = await regex.ReplaceAsync(text, async x =>
                {
                    var value = await FindParameterValue(x, cmsRenderer, recurse).ConfigureAwait(false);

                    if (value == null) return "";

                    var enumerableValue = value as IEnumerable;
                    if (enumerableValue == null || value is string)
                        return value.ToString();

                    var stringValues = enumerableValue.OfType<object>().Select(y => y.ToString()).ToList();

                    return string.Join(SeperateListItemsWith, stringValues);
                }).ConfigureAwait(false);
            }

            return text;
        }
        
        protected abstract Task<object> FindParameterValue(Match match, ICmsRenderer cmsRenderer, Func<string, Task<string>> recurse);
        protected abstract IEnumerable<Regex> GetRegexes();
    }
}