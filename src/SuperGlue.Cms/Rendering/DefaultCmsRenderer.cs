using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using SuperGlue.Cms.Parsing;

namespace SuperGlue.Cms.Rendering
{
    public class DefaultCmsRenderer : ICmsRenderer
    {
        private readonly IEnumerable<ITextParser> _textParsers;
        private readonly IEnumerable<IExecuteDataSource> _executeDataSources;
        private static readonly Cache<string, CompiledText> CompiledTexts = new Cache<string, CompiledText>();

        public DefaultCmsRenderer(IEnumerable<ITextParser> textParsers, IEnumerable<IExecuteDataSource> executeDataSources)
        {
            _textParsers = textParsers;
            _executeDataSources = executeDataSources;
        }

        public Task<CompiledText> Compile(string text, IDictionary<string, object> environment)
        {
            return CompiledTexts.GetAsync(CalculateHash(text), async hash =>
            {
                var parsers = _textParsers.ToList();
                var currentText = new CompiledText(text, new ReadOnlyDictionary<string, CompiledText.DataSource>(new Dictionary<string, CompiledText.DataSource>()));

                foreach (var parser in parsers)
                {
                    var dataSources = currentText.DataSources.ToDictionary(x => x.Key, x => x.Value);

                    try
                    {
                        var compiled = await parser.Compile(currentText.Body, environment, async x =>
                        {
                            var recurseCompiled = await Compile(x, environment).ConfigureAwait(false);

                            foreach (var dataSource in recurseCompiled.DataSources)
                                dataSources[dataSource.Key] = dataSource.Value;

                            return recurseCompiled.Body;
                        }).ConfigureAwait(false);

                        foreach (var dataSource in compiled.DataSources)
                            dataSources[dataSource.Key] = dataSource.Value;

                        currentText = new CompiledText(compiled.Body, new ReadOnlyDictionary<string, CompiledText.DataSource>(dataSources));
                    }
                    catch (Exception ex)
                    {
                        environment.Log(ex, $"Failed compiling text with parser: {parser.GetType().FullName}", LogLevel.Warn);
                    }
                }

                return currentText;
            });
        }

        public async Task<string> Render(CompiledText text, IDictionary<string, object> environment, IReadOnlyDictionary<string, dynamic> dataSources = null)
        {
            var currentDataSources = new Dictionary<string, dynamic>
            {
                ["Environment"] = environment
            };

            if (dataSources != null)
            {
                foreach (var dataSource in dataSources)
                    currentDataSources[dataSource.Key] = dataSource.Value;
            }

            foreach (var dataSource in text.DataSources)
            {
                var executor = _executeDataSources.FirstOrDefault(x => x.Type == dataSource.Value.Type);

                if (executor == null)
                {
                    environment.Log($"Can't find executor for datasource: {dataSource.Key} of type {dataSource.Value.Type}", LogLevel.Warn);

                    continue;
                }

                currentDataSources[dataSource.Key] = await executor.Execute(dataSource.Value.Settings).ConfigureAwait(false);
            }

            var parsers = _textParsers.ToList();

            var currentText = text.Body;

            foreach (var parser in parsers)
            {
                try
                {
                    currentText = await parser.Render(currentText, environment, currentDataSources).ConfigureAwait(false);
                }
                catch (Exception ex)
                {
                    environment.Log(ex, $"Failed rendering text with parser: {parser.GetType().FullName}", LogLevel.Warn);
                }
            }

            return currentText;
        }

        private static string CalculateHash(string input)
        {
            var md5 = MD5.Create();

            var inputBytes = Encoding.ASCII.GetBytes(input);

            var hash = md5.ComputeHash(inputBytes);

            var sb = new StringBuilder();

            foreach (var t in hash)
                sb.Append(t.ToString("X2"));

            return sb.ToString();
        }
    }
}