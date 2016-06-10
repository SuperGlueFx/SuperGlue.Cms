using System.Collections.Generic;

namespace SuperGlue.Cms.Localization
{
    public class DefaultLocalizationNamespaceFinder : IFindCurrentLocalizationNamespace
    {
        private readonly IEnumerable<string> _pathEnvironmentKeys = new List<string>
        {
            "owin.RequestPath"
        };

        public string Find(IDictionary<string, object> environment)
        {
            foreach (var key in _pathEnvironmentKeys)
            {
                var value = environment.Get<string>(key);

                if(string.IsNullOrEmpty(value))
                    continue;

                return value.Replace("/", ":").Replace(".", "");
            }

            return "";
        }
    }
}