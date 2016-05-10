using System;
using System.Collections.Generic;
using System.Linq;
using SuperGlue.Cms.Components;
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

        public virtual string RenderComponent(ICmsComponent component, IDictionary<string, object> settings)
        {
            return component.Render(settings);
        }

        public virtual string RenderTemplate(CmsTemplate template, IDictionary<string, object> settings)
        {
            return ParseText($"<md>{template.Body}</md>");
        }

        public virtual string ParseText(string text, ParseTextOptions options = null)
        {
            var parsers = _textParsers.ToList();

            if (options != null && options.UseOnlyParsersTagged.Any())
                parsers = parsers.Where(x => options.UseOnlyParsersTagged.All(y => x.GetTags().Contains(y))).ToList();

            foreach (var textParser in parsers)
            {
                var useOptionsForNextLevel = options != null && !options.FilterOnlyFirstLevel;

                try
                {
                    text = textParser.Parse(text, this, x => ParseText(x, useOptionsForNextLevel ? options : null));
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