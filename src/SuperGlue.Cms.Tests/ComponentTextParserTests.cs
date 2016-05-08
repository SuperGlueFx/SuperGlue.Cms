using System.Collections.Generic;
using System.IO;
using System.Linq;
using Should;
using SuperGlue.Cms.Components;
using SuperGlue.Cms.Parsing;
using SuperGlue.Cms.Rendering;

namespace SuperGlue.Cms.Tests
{
    public class ComponentTextParserTests
    {
        private readonly ComponentTextParser _componentTextParser;

        public ComponentTextParserTests()
        {
            _componentTextParser = new ComponentTextParser(new List<ICmsComponent>
            {
                new FakeComponent()
            });
        }

        public void when_parsing_component_with_settings()
        {
            var result = Parse(@"![Components.FakeComponent Settings={Test:""Test"", Asd:1, Dsa:""asd-dsa""}]!");

            result.ShouldEqual("Test=Test, Asd=1, Dsa=asd-dsa");
        }

        private string Parse(string input)
        {
            var renderer = new FakeCmsRenderer(new List<ITextParser> { _componentTextParser });

            return _componentTextParser.Parse(input, renderer, new DefaultCmsContext(x => null, null), x => x);
        }
    }

    public class FakeComponent : ICmsComponent
    {
        public string Name => GetType().Name;
        public string Category => "Test";

        public IRenderInformation Render(ICmsContext context, IDictionary<string, object> settings)
        {
            var text = string.Join(", ", settings.Select(x => $"{x.Key}={x.Value}"));

            return new TextRenderInformation(Enumerable.Empty<RequestContext>(), text, "text/plain");
        }

        public IDictionary<string, object> GetDefaultSettings()
        {
            return new Dictionary<string, object>();
        }
    }

    public class FakeCmsRenderer : DefaultCmsRenderer
    {
        public FakeCmsRenderer(IEnumerable<ITextParser> textParsers) : base(textParsers)
        {
        }

        public override IRenderResult RenderComponent(ICmsComponent component, IDictionary<string, object> settings, ICmsContext context)
        {
            var result = component.Render(context, settings) as TextRenderInformation;

            if(result == null)
                return new RenderResult("", "");

            return new RenderResult(result.ContentType, result.Text);
        }

        private class RenderResult : IRenderResult
        {
            private readonly string _text;

            public RenderResult(string contentType, string text)
            {
                ContentType = contentType;
                _text = text;
            }

            public string ContentType { get; }

            public void RenderTo(TextWriter writer)
            {
                writer.Write(_text);
            }

            public string Read()
            {
                return _text;
            }
        }
    }
}