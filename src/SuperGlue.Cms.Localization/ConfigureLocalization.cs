using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using SuperGlue.Configuration;
using SuperGlue.Configuration.Ioc;

namespace SuperGlue.Cms.Localization
{
    public class ConfigureLocalization : ISetupConfigurations
    {
        public IEnumerable<ConfigurationSetupResult> Setup(string applicationEnvironment)
        {
            yield return new ConfigurationSetupResult("superglue.Cms.LocalizationSetup", environment =>
            {
                environment.AlterSettings<IocConfiguration>(x => x.Register(typeof(IFindCurrentLocalizationNamespace), typeof(DefaultLocalizationNamespaceFinder))
                    .Register(typeof(ILocalizeText), typeof(DefaultLocalizer))
                    .Register(typeof(CultureInfo), (y, z) => environment.GetSettings<LocalizationSettings>().GetCulture(environment)));

                return Task.CompletedTask;
            }, "superglue.ContainerSetup");
        }
    }
}