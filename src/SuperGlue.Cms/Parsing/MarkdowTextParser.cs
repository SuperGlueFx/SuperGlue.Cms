using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using MarkdownSharp;
using SuperGlue.Cms.Rendering;

namespace SuperGlue.Cms.Parsing
{
    public class MarkdowTextParser : RegexTextParser
    {
        public override IEnumerable<string> GetTags()
        {
            yield return "markdown";
            yield return "tohtml";
            yield return "singletarget";
        }

        protected override object FindParameterValue(Match match, ICmsRenderer cmsRenderer, Func<string, string> recurse)
        {
            var markdownParser = new Markdown(new MarkdownOptions
            {
                AutoNewLines = true
            });

            var mardown = match.Groups[1].Value;

            return string.IsNullOrEmpty(mardown) ? mardown : markdownParser.Transform(mardown);
        }

        protected override IEnumerable<Regex> GetRegexes()
        {
            yield return new Regex(@"\<md\>(.*?)\<\/md\>", RegexOptions.Compiled | RegexOptions.Singleline);
        }
    }
}