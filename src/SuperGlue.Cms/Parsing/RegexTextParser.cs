using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using SuperGlue.Cms.Rendering;

namespace SuperGlue.Cms.Parsing
{
    public abstract class RegexTextParser : ITextParser
    {
        public virtual void SetUp(ISetupTextParser setup)
        {
            
        }

        public async Task<CompiledText> Compile(string text, IDictionary<string, object> environment, Func<string, Task<string>> recurse)
        {
            text = text ?? "";
            var dataSources = new Dictionary<string, CompiledText.DataSource>();

            environment.Log($"Going to compile using regex parser: {GetType().FullName}", LogLevel.Debug);

            foreach (var regex in GetRegexes())
            {
                environment.Log($"Replacing using parser: {GetType().FullName} with regex: {regex}", LogLevel.Debug);

                text = await regex.ReplaceAsync(text, async x =>
                {
                    environment.Log($"Regex parser: {GetType().FullName} found a match", LogLevel.Debug);

                    var compiled = await CompileInner(x, environment, recurse).ConfigureAwait(false);

                    foreach (var dataSource in compiled.DataSources)
                        dataSources[dataSource.Key] = dataSource.Value;

                    return compiled.Body;
                }).ConfigureAwait(false);
            }

            return new CompiledText(text, new ReadOnlyDictionary<string, CompiledText.DataSource>(dataSources));
        }

        public Task<string> Render(string text, IDictionary<string, object> environment, IReadOnlyDictionary<string, dynamic> dataSources)
        {
            return Task.FromResult(text);
        }

        protected abstract Task<CompiledText> CompileInner(Match match, IDictionary<string, object> environment, Func<string, Task<string>> recurse);
        protected abstract IEnumerable<Regex> GetRegexes();
    }
}