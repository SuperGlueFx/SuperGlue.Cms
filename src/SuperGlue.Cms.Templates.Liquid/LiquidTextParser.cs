using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using DotLiquid;
using SuperGlue.Cms.Parsing;
using SuperGlue.Cms.Rendering;

namespace SuperGlue.Cms.Templates.Liquid
{
    public class LiquidTextParser : ITextParser
    {
        private static readonly Cache<string, Template> ParsedTemplates = new Cache<string, Template>();

        public void SetUp()
        {
            
        }

        public Task<CompiledText> Compile(string text, IDictionary<string, object> environment, Func<string, Task<string>> recurse)
        {
            return Task.FromResult(new CompiledText(text, new ReadOnlyDictionary<string, CompiledText.DataSource>(new Dictionary<string, CompiledText.DataSource>())));
        }

        public Task<string> Render(string text, IDictionary<string, object> environment, IReadOnlyDictionary<string, dynamic> dataSources)
        {
            var hash = CalculateHash(text);

            var template = ParsedTemplates.Get(hash, x => Template.Parse(text));

            return Task.FromResult(template.Render(Hash.FromDictionary(dataSources.ToDictionary(x => x.Key, x => x.Value))));
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