using SuperGlue.Cms.Localization;

namespace SuperGlue.Cms.Tests
{
    public class FakeNamespaceFinder : IFindCurrentLocalizationNamespace
    {
        public string Find()
        {
            return "";
        }
    }
}