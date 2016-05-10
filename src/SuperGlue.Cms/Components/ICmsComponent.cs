using System.Collections.Generic;
using System.Threading.Tasks;

namespace SuperGlue.Cms.Components
{
    public interface ICmsComponent
    {
        string Name { get; }
        string Category { get; }

        Task<string> Render(IDictionary<string, object> settings);
        IDictionary<string, object> GetDefaultSettings();
    }
}