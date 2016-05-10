using System.Collections.Generic;
using System.Threading.Tasks;
using SuperGlue.Configuration;

namespace SuperGlue.Cms.Components
{
    public class ConfigureComponents : ISetupConfigurations
    {
        public IEnumerable<ConfigurationSetupResult> Setup(string applicationEnvironment)
        {
            yield return new ConfigurationSetupResult("superglue.Cms.ComponentsSetup", environment =>
            {
                environment.RegisterAll(typeof(ICmsComponent));

                return Task.CompletedTask;
            }, "superglue.ContainerSetup");
        }
    }
}