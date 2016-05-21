using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SuperGlue.Cms.Rendering;

namespace SuperGlue.Cms.Parsing
{
    public interface ITextParser
    {
        Task<string> Parse(string text, ICmsRenderer cmsRenderer, IReadOnlyDictionary<string, dynamic> dataSources, Func<string, Task<string>> recurse);
    }
}