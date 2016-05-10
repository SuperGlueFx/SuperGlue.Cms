using System.Collections.Generic;
using System.Threading.Tasks;
using SuperGlue.Configuration;

namespace SuperGlue.Cms.Templates
{
    public class ConfigureTemplates : ISetupConfigurations
    {
        public IEnumerable<ConfigurationSetupResult> Setup(string applicationEnvironment)
        {
            yield return new ConfigurationSetupResult("superglue.Cms.TemplatesSetup", environment =>
            {
                environment.RegisterTransient(typeof(ITemplateStorage), typeof(FileSystemTemplateStorage));
                environment.AlterSettings<FileSystemTemplatesSettings>(x => x.TemplateFolder = "~/templates");

                return Task.CompletedTask;
            }, "superglue.ContainerSetup");
        }
    }
}