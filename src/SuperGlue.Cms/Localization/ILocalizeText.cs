using System.Globalization;
using System.Threading.Tasks;

namespace SuperGlue.Cms.Localization
{
    public interface ILocalizeText
    {
        Task<string> Localize(string key, CultureInfo culture);
        Task Load();
    }
}