using System.Collections.Generic;
using SuperGlue.Cms.Rendering;

namespace SuperGlue.Cms.Components
{
    public interface ICmsComponent
    {
        string Name { get; }
        string Category { get; }

        IRenderInformation Render(ICmsContext context, IDictionary<string, object> settings);
        IDictionary<string, object> GetDefaultSettings();
    }
}