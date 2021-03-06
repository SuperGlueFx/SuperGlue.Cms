using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using MarkdownSharp;
using SuperGlue.Cms.Parsing;
using SuperGlue.Cms.Rendering;

namespace SuperGlue.Cms.Templates.Markdown
{
    public class MarkdowTextParser : RegexTextParser
    {
        protected override Task<CompiledText> CompileInner(Match match, IDictionary<string, object> environment, Func<string, Task<string>> recurse)
        {
            var markdownParser = new MarkdownSharp.Markdown(new MarkdownOptions
            {
                AutoNewLines = true
            });

            var mardown = match.Groups[1].Value;

            return Task.FromResult(new CompiledText(string.IsNullOrEmpty(mardown) ? mardown : markdownParser.Transform(mardown)));
        }

        protected override IEnumerable<Regex> GetRegexes()
        {
            yield return new Regex(@"\<md\>(.*?)\<\/md\>", RegexOptions.Compiled | RegexOptions.Singleline);
        }
    }
}