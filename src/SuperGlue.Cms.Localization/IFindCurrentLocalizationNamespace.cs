using System.Collections.Generic;
using System.Threading.Tasks;

namespace SuperGlue.Cms.Localization
{
    public interface IFindCurrentLocalizationNamespace
    {
        Task<string> Find(IDictionary<string, object> environment);
    }
}