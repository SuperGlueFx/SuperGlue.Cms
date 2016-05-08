using System.Globalization;
using SuperGlue.Cms.Localization;

namespace SuperGlue.Cms.Tests
{
    public class FakeLocalizer : ILocalizeText
    {
        public string Localize(string key, CultureInfo culture)
        {
            if (key == "test")
                return "asd {replaceme} asd";

            return "";
        }

        public void Load()
        {

        }
    }
}