using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using SuperGlue.Cms.Parsing;

namespace SuperGlue.Cms.Rendering
{
    public class DefaultCmsRenderer : ICmsRenderer
    {
        private readonly IEnumerable<ITextParser> _textParsers;

        public DefaultCmsRenderer(IEnumerable<ITextParser> textParsers)
        {
            _textParsers = textParsers;
        }

        public virtual async Task<string> ParseText(string text, IDictionary<string, object> environment, IDictionary<string, dynamic> dataSources = null)
        {
            var parsers = _textParsers.ToList();

            dataSources = dataSources ?? new Dictionary<string, dynamic>();

            foreach (var textParser in parsers)
            {
                try
                {
                    text = await textParser.Parse(text, this, environment, new ReadOnlyDictionary<string, dynamic>(dataSources)).ConfigureAwait(false);
                }
                catch (Exception ex)
                {
                    environment.Log(ex, $"Failed parsing text with parser: {textParser.GetType().FullName}", LogLevel.Warn);
                }
            }

            return text;
        }
    }
}