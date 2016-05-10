using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using MarkdownSharp;
using SuperGlue.Cms.Rendering;

namespace SuperGlue.Cms.Parsing
{
    public class MarkdowTextParser : RegexTextParser
    {
        protected override Task<object> FindParameterValue(Match match, ICmsRenderer cmsRenderer, Func<string, Task<string>> recurse)
        {
            var markdownParser = new Markdown(new MarkdownOptions
            {
                AutoNewLines = true
            });

            var mardown = match.Groups[1].Value;

            return Task.FromResult<object>(string.IsNullOrEmpty(mardown) ? mardown : markdownParser.Transform(mardown));
        }

        protected override IEnumerable<Regex> GetRegexes()
        {
            yield return new Regex(@"\<md\>(.*?)\<\/md\>", RegexOptions.Compiled | RegexOptions.Singleline);
        }
    }
}