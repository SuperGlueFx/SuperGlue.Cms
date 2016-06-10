using System.Collections.Generic;

namespace SuperGlue.Cms.Localization
{
    public interface IFindCurrentLocalizationNamespace
    {
        string Find(IDictionary<string, object> environment);
    }
}