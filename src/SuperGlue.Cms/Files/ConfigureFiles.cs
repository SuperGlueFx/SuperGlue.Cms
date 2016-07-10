using System.Collections.Generic;
using System.Threading.Tasks;
using SuperGlue.Configuration;
using SuperGlue.Configuration.Ioc;

namespace SuperGlue.Cms.Files
{
    public class ConfigureFiles : ISetupConfigurations
    {
        public IEnumerable<ConfigurationSetupResult> Setup(string applicationEnvironment)
        {
            yield return new ConfigurationSetupResult("superglue.Cms.FilesSetup", environment =>
            {
                environment.AlterSettings<IocConfiguration>(x => x.Register(typeof(IUploadFiles), typeof(FileSystemFileUploader)));

                return Task.CompletedTask;
            }, "superglue.ContainerSetup");
        }
    }
}