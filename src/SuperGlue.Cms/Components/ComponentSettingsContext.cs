using System.Collections.Generic;

namespace SuperGlue.Cms.Components
{
    public class ComponentSettingsContext
    {
        public static RequestContext Build(IDictionary<string, object> settings)
        {
            return new RequestContext(typeof(ComponentSettingsContext).Name, new Dictionary<string, object>
            {
                {"Settings", settings}
            });
        }
    }
}