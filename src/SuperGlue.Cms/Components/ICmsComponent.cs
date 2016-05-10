using System.Collections.Generic;

namespace SuperGlue.Cms.Components
{
    public interface ICmsComponent
    {
        string Name { get; }
        string Category { get; }

        string Render(IDictionary<string, object> settings);
        IDictionary<string, object> GetDefaultSettings();
    }
}