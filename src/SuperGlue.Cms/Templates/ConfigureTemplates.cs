using System.Collections.Generic;
using System.Threading.Tasks;
using SuperGlue.Configuration;
using SuperGlue.Configuration.Ioc;

namespace SuperGlue.Cms.Templates
{
    public class ConfigureTemplates : ISetupConfigurations
    {
        public IEnumerable<ConfigurationSetupResult> Setup(string applicationEnvironment)
        {
            yield return new ConfigurationSetupResult("superglue.Cms.TemplatesSetup", environment =>
            {
                environment.AlterSettings<IocConfiguration>(x => x.Register(typeof(ITemplateStorage), typeof(FileSystemTemplateStorage)));

                environment.AlterSettings<FileSystemTemplatesSettings>(x => x.TemplateFolder = "~/templates");

                return Task.CompletedTask;
            }, "superglue.ContainerSetup");
        }
    }
}