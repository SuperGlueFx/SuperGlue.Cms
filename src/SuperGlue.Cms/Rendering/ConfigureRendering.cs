using System.Collections.Generic;
using System.Threading.Tasks;
using SuperGlue.Configuration;

namespace SuperGlue.Cms.Rendering
{
    public class ConfigureRendering : ISetupConfigurations
    {
        public IEnumerable<ConfigurationSetupResult> Setup(string applicationEnvironment)
        {
            yield return new ConfigurationSetupResult("superglue.Cms.RenderingSetup", environment =>
            {
                environment.RegisterTransient(typeof(ICmsRenderer), typeof(DefaultCmsRenderer));
                environment.RegisterAll(typeof(IExecuteDataSource));

                return Task.CompletedTask;
            }, "superglue.ContainerSetup");
        }
    }
}