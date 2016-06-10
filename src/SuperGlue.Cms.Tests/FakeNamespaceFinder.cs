using System.Collections.Generic;
using SuperGlue.Cms.Localization;

namespace SuperGlue.Cms.Tests
{
    public class FakeNamespaceFinder : IFindCurrentLocalizationNamespace
    {
        public string Find(IDictionary<string, object> environment)
        {
            return "";
        }
    }
}