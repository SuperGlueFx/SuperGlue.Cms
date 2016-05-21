using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SuperGlue.Cms.Parsing;
using SuperGlue.Cms.Templates;

namespace SuperGlue.Cms.Rendering
{
    public class DefaultCmsRenderer : ICmsRenderer
    {
        private readonly IEnumerable<ITextParser> _textParsers;

        public DefaultCmsRenderer(IEnumerable<ITextParser> textParsers)
        {
            _textParsers = textParsers;
        }

        public virtual Task<string> RenderTemplate(CmsTemplate template)
        {
            return Task.FromResult($"<md>{template.Body}</md>");
        }

        public virtual async Task<string> ParseText(string text, ParseTextOptions options = null)
        {
            var parsers = _textParsers.ToList();

            foreach (var textParser in parsers)
            {
                var useOptionsForNextLevel = options != null && !options.FilterOnlyFirstLevel;

                try
                {
                    text = await textParser.Parse(text, this, x => ParseText(x, useOptionsForNextLevel ? options : null)).ConfigureAwait(false);
                }
                catch (Exception)
                {
                    //TODO:Log exception
                }
            }

            return text;
        }
    }
}