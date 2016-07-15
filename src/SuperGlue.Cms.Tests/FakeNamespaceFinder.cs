using System.Collections.Generic;
using System.Threading.Tasks;
using SuperGlue.Cms.Localization;

namespace SuperGlue.Cms.Tests
{
    public class FakeNamespaceFinder : IFindCurrentLocalizationNamespace
    {
        public Task<string> Find(IDictionary<string, object> environment)
        {
            return Task.FromResult("");
        }
    }
}