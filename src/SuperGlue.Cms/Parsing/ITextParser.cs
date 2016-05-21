using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SuperGlue.Cms.Rendering;

namespace SuperGlue.Cms.Parsing
{
    public interface ITextParser
    {
        Task<CompiledText> Compile(string text, IDictionary<string, object> environment, Func<string, Task<string>> recurse);
        Task<string> Render(string text, IDictionary<string, object> environment, IReadOnlyDictionary<string, dynamic> dataSources);
    }
}