using System;
using System.Collections.Generic;
using System.IO;
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

        public virtual IRenderResult RenderComponent(ICmsComponent component, IDictionary<string, object> settings, ICmsContext context)
        {
            if (!context.CanRender(component))
                return new RenderResult("text/plain", x => { }, new Dictionary<Guid, RequestContext>(), context);

            var renderInformation = component.Render(context, settings);

            if (renderInformation == null)
                return new RenderResult("text/plain", x => { }, new Dictionary<Guid, RequestContext>(), context);

            var contexts = renderInformation.Contexts.ToDictionary(x => Guid.NewGuid(), x => x);
            contexts[Guid.NewGuid()] = ComponentSettingsContext.Build(settings);

            return new RenderResult(renderInformation.ContentType, x =>
            {
                var renderer = (IRenderer)context.Resolve(typeof(Renderer<>).MakeGenericType(renderInformation.GetType()));

                renderer.Render(renderInformation, context, x);
            }, contexts, context);
        }

        public virtual IRenderResult RenderTemplate(CmsTemplate template, IDictionary<string, object> settings, ICmsContext context)
        {
            if (!context.CanRender(template))
                return new RenderResult("text/plain", x => { }, new Dictionary<Guid, RequestContext>(), context);

            var contexts = new Dictionary<Guid, RequestContext>();

            var result = ParseText($"<md>{template.Body}</md>", context);

            return new RenderResult(string.IsNullOrEmpty(template.ContentType) ? result.ContentType : template.ContentType, x => result.RenderTo(x), contexts, context);
        }

        public virtual IRenderResult ParseText(string text, ICmsContext context, ParseTextOptions options = null)
        {
            var parsers = _textParsers.ToList();

            if (options != null && options.UseOnlyParsersTagged.Any())
                parsers = parsers.Where(x => options.UseOnlyParsersTagged.All(y => x.GetTags().Contains(y))).ToList();

            foreach (var textParser in parsers)
            {
                var useOptionsForNextLevel = options != null && !options.FilterOnlyFirstLevel;

                try
                {
                    text = textParser.Parse(text, this, context, x => ParseText(x, context, useOptionsForNextLevel ? options : null).Read());
                }
                catch (Exception)
                {
                    //TODO:Log exception
                }
            }

            return new RenderResult("text/html", x => x.Write(text), new Dictionary<Guid, RequestContext>(), context);
        }

        private interface IRenderer
        {
            void Render(IRenderInformation information, ICmsContext context, TextWriter renderTo);
        }

        private class Renderer<TRenderInformation> : IRenderer where TRenderInformation : IRenderInformation
        {
            private readonly ICmsRenderer<TRenderInformation> _renderer;

            public Renderer(ICmsRenderer<TRenderInformation> renderer)
            {
                _renderer = renderer;
            }

            public void Render(IRenderInformation information, ICmsContext context, TextWriter renderTo)
            {
                _renderer.Render((TRenderInformation)information, context, renderTo);
            }
        }

        private class RenderResult : IRenderResult
        {
            private readonly Action<TextWriter> _render;
            private readonly IDictionary<Guid, RequestContext> _requestContexts;
            private readonly ICmsContext _cmsContext;

            public RenderResult(string contentType, Action<TextWriter> render, IDictionary<Guid, RequestContext> requestContexts, ICmsContext cmsContext)
            {
                ContentType = contentType;
                _render = render;
                _requestContexts = requestContexts;
                _cmsContext = cmsContext;
            }

            public string ContentType { get; }

            public void RenderTo(TextWriter writer)
            {
                foreach (var context in _requestContexts)
                    _cmsContext.EnterContext(context.Key, context.Value);

                _render(writer);

                foreach (var context in _requestContexts)
                    _cmsContext.ExitContext(context.Key);
            }

            public string Read()
            {
                var stream = new MemoryStream();
                var writer = new StreamWriter(stream);

                RenderTo(writer);

                writer.Flush();
                stream.Position = 0;

                using (var reader = new StreamReader(stream))
                    return reader.ReadToEnd();
            }
        }
    }
}