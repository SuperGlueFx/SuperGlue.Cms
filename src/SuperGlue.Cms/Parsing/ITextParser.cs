using System.Collections.Generic;
using System.Threading.Tasks;
using SuperGlue.Cms.Rendering;

namespace SuperGlue.Cms.Parsing
{
    public interface ITextParser
    {
        Task<string> Parse(string text, ICmsRenderer cmsRenderer, IDictionary<string, object> environment, IReadOnlyDictionary<string, dynamic> dataSources);
    }
}