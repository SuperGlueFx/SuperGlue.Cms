using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Threading.Tasks;
using Should;
using SuperGlue.Cms.Localization;
using SuperGlue.Cms.Parsing;
using SuperGlue.Cms.Rendering;

namespace SuperGlue.Cms.Tests
{
    public class LocalizationTextParserTests
    {
        private readonly LocalizationTextParser _parser;

        public LocalizationTextParserTests()
        {
            _parser = new LocalizationTextParser(new FakeLocalizer(), new FakeNamespaceFinder(), new CultureInfo("sv-SE"));
        }

        public void when_parsing_translation_with_existing_replacement()
        {
            var result = Parse("%[test replacements=replaceme:dsa]%");

            result.ShouldEqual("asd dsa asd");
        }

        private string Parse(string input)
        {
            return _parser.Parse(input,
                new DefaultCmsRenderer(new List<ITextParser> { _parser }),
                new Dictionary<string, object>(), 
                new ReadOnlyDictionary<string, dynamic>(new Dictionary<string, dynamic>())).Result;
        }
    }
}
