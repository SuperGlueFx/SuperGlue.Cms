using System.Collections.Generic;
using System.Threading.Tasks;
using SuperGlue.Configuration;

namespace SuperGlue.Cms.Files
{
    public class ConfigureFiles : ISetupConfigurations
    {
        public IEnumerable<ConfigurationSetupResult> Setup(string applicationEnvironment)
        {
            yield return new ConfigurationSetupResult("superglue.Cms.FilesSetup", environment =>
            {
                environment.RegisterTransient(typeof(IUploadFiles), typeof(FileSystemFileUploader));

                return Task.CompletedTask;
            }, "superglue.ContainerSetup");
        }
    }
}