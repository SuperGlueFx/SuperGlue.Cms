using System.Collections.Generic;
using System.Threading.Tasks;
using Should;
using SuperGlue.Cms.Parsing;
using SuperGlue.Cms.Rendering;

namespace SuperGlue.Cms.Tests
{
    public class DataSourceTextParserTests
    {
        private readonly DataSourceTextParser _parser;

        public DataSourceTextParserTests()
        {
            _parser = new DataSourceTextParser();
        }

        public void when_parsing_existing_data_source()
        {
            var result = Parse("![DataSource.TestType.MyName settings={}]!");

            result.DataSources.Count.ShouldEqual(1);
            result.DataSources.Keys.ShouldContain("MyName");
            result.DataSources["MyName"].Type.ShouldEqual("TestType");
        }

        private CompiledText Parse(string input)
        {
            return _parser.Compile(input,
                new Dictionary<string, object>(),
                Task.FromResult)
                .Result;
        }
    }
}