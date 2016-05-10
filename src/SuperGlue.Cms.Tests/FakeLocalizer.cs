using System.Globalization;
using System.Threading.Tasks;
using SuperGlue.Cms.Localization;

namespace SuperGlue.Cms.Tests
{
    public class FakeLocalizer : ILocalizeText
    {
        public Task<string> Localize(string key, CultureInfo culture)
        {
            if (key == "test")
                return Task.FromResult("asd {replaceme} asd");

            return Task.FromResult("");
        }

        public Task Load()
        {
            return Task.CompletedTask;
        }
    }
}