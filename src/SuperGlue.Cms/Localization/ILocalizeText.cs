using System.Globalization;

namespace SuperGlue.Cms.Localization
{
    public interface ILocalizeText
    {
        string Localize(string key, CultureInfo culture);
        void Load();
    }
}