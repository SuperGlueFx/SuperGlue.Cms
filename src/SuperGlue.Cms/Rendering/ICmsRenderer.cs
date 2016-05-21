using System.Collections.Generic;
using System.Threading.Tasks;

namespace SuperGlue.Cms.Rendering
{
    public interface ICmsRenderer
    {
        Task<CompiledText> Compile(string text, IDictionary<string, object> environment);
        Task<string> Render(CompiledText text, IDictionary<string, object> environment, IReadOnlyDictionary<string, dynamic> dataSources = null);
    }
}