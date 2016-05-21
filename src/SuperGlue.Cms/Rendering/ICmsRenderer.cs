using System.Collections.Generic;
using System.Threading.Tasks;

namespace SuperGlue.Cms.Rendering
{
    public interface ICmsRenderer
    {
        Task<string> ParseText(string text, IDictionary<string, object> environment, IDictionary<string, dynamic> dataSources = null);
    }
}