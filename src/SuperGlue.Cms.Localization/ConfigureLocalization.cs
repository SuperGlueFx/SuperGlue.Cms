using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using SuperGlue.Configuration;

namespace SuperGlue.Cms.Localization
{
    public class ConfigureLocalization : ISetupConfigurations
    {
        public IEnumerable<ConfigurationSetupResult> Setup(string applicationEnvironment)
        {
            yield return new ConfigurationSetupResult("superglue.Cms.LocalizationSetup", environment =>
            {
                environment.RegisterAll(typeof(ILocalizationVisitor));
                environment.RegisterTransient(typeof(IFindCurrentLocalizationNamespace), typeof(DefaultLocalizationNamespaceFinder));
                environment.RegisterTransient(typeof(ILocalizeText), typeof(DefaultLocalizer));
                environment.RegisterTransient(typeof(CultureInfo), (x, y) => y.GetSettings<LocalizationSettings>().GetCulture(y));

                return Task.CompletedTask;
            }, "superglue.ContainerSetup");
        }
    }
}