using System.Collections.Generic;
using System.Threading.Tasks;

namespace SuperGlue.Cms.Localization
{
    public class DefaultLocalizationNamespaceFinder : IFindCurrentLocalizationNamespace
    {
        private readonly IEnumerable<string> _pathEnvironmentKeys = new List<string>
        {
            "owin.RequestPath"
        };

        public Task<string> Find(IDictionary<string, object> environment)
        {
            foreach (var key in _pathEnvironmentKeys)
            {
                var value = environment.Get<string>(key);

                if(string.IsNullOrEmpty(value))
                    continue;

                return Task.FromResult(value.Replace("/", ":").Replace(".", ""));
            }

            return Task.FromResult("");
        }
    }
}