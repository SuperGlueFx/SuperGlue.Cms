using System.Collections.Generic;
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
            var renderer = new DefaultCmsRenderer(new List<ITextParser> { _componentTextParser });

            return _componentTextParser.Parse(input, renderer, x => x);
        }
    }

    public class FakeComponent : ICmsComponent
    {
        public string Name => GetType().Name;
        public string Category => "Test";

        public string Render(IDictionary<string, object> settings)
        {
            return string.Join(", ", settings.Select(x => $"{x.Key}={x.Value}"));
        }

        public IDictionary<string, object> GetDefaultSettings()
        {
            return new Dictionary<string, object>();
        }
    }
}