using System.Collections.Generic;
using System.Threading.Tasks;
using SuperGlue.Configuration;
using SuperGlue.Configuration.Ioc;

namespace SuperGlue.Cms.Rendering
{
    public class ConfigureRendering : ISetupConfigurations
    {
        public IEnumerable<ConfigurationSetupResult> Setup(string applicationEnvironment)
        {
            yield return new ConfigurationSetupResult("superglue.Cms.RenderingSetup", environment =>
            {
                environment.AlterSettings<IocConfiguration>(x => x.Register(typeof(ICmsRenderer), typeof(DefaultCmsRenderer))
                    .Scan(typeof(IExecuteDataSource)));

                return Task.CompletedTask;
            }, "superglue.ContainerSetup");
        }
    }
}