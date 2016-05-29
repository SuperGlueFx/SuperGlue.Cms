using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using SuperGlue.Caching;
using SuperGlue.Cms.Parsing;
using SuperGlue.Configuration;

namespace SuperGlue.Cms.Rendering
{
	public class DefaultCmsRenderer : ICmsRenderer
	{
		private readonly IEnumerable<ITextParser> _textParsers;
		private readonly IEnumerable<IExecuteDataSource> _executeDataSources;
		private readonly ICache _cache;

		public DefaultCmsRenderer(IEnumerable<ITextParser> textParsers, IEnumerable<IExecuteDataSource> executeDataSources,
			ICache cache)
		{
			_textParsers = textParsers;
			_executeDataSources = executeDataSources;
			_cache = cache;
		}

		public async Task<CompiledText> Compile(string text, IDictionary<string, object> environment)
		{
		    var setupTextParser = new SetupTextParser(environment);

            var parsers = _textParsers.ToList();

		    foreach (var parser in parsers)
		    {
		        environment.Log($"Setting up using parser {parser.GetType().FullName}", LogLevel.Debug);

                parser.SetUp(setupTextParser);
		    }

		    var key = setupTextParser.GetCacheKey(text);

			var compiledText = await _cache.Get<CompiledText>(key).ConfigureAwait(false);

			if (compiledText != null)
				return compiledText;

			var currentText = new CompiledText(text, new ReadOnlyDictionary<string, CompiledText.DataSource>(new Dictionary<string, CompiledText.DataSource>()));

			foreach (var parser in parsers)
			{
				var dataSources = currentText.DataSources.ToDictionary(x => x.Key, x => x.Value);

                environment.Log($"Compiling using parser: {parser.GetType().FullName}", LogLevel.Debug);

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

			await _cache.Set(key, currentText, environment.GetSettings<RenderingSettings>().CompiliationCacheTimeout).ConfigureAwait(false);

			return currentText;
		}

		public async Task<string> Render(CompiledText text, IDictionary<string, object> environment,
			IReadOnlyDictionary<string, dynamic> dataSources = null)
		{
			var currentDataSources = new Dictionary<string, dynamic>();

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
					environment.Log($"Can't find executor for datasource: {dataSource.Key} of type {dataSource.Value.Type}",
						LogLevel.Warn);

					continue;
				}

				currentDataSources[dataSource.Key] = await executor.Execute(dataSource.Value.Settings).ConfigureAwait(false);
			}

			var parsers = _textParsers.ToList();

			var currentText = text.Body;

			foreach (var parser in parsers)
			{
                environment.Log($"Parsing using parser: {parser.GetType().FullName}", LogLevel.Debug);

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

	    private class SetupTextParser : ISetupTextParser
	    {
	        private readonly IDictionary<string, object> _environment;
            private readonly StringBuilder _cacheKeyBuilder = new StringBuilder();

	        public SetupTextParser(IDictionary<string, object> environment)
	        {
	            _environment = environment;
	        }

	        public void DependsOn(Func<IDictionary<string, object>, object> func)
	        {
	            _cacheKeyBuilder.Append(func(_environment));
	        }

	        public string GetCacheKey(string input)
	        {
	            return CalculateHash($"{input}-{_cacheKeyBuilder}");
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
}