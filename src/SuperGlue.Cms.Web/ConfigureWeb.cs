using System.Collections.Generic;
using System.Threading.Tasks;
using SuperGlue.Configuration;
using SuperGlue.Web.Output;

namespace SuperGlue.Cms.Web
{
    public class ConfigureWeb : ISetupConfigurations
    {
        public IEnumerable<ConfigurationSetupResult> Setup(string applicationEnvironment)
        {
            yield return new ConfigurationSetupResult("superglue.Cms.WebSetup", environment =>
            {
                environment.RegisterTransient(typeof(IWriteToOutput), typeof(CmsOutputWriter));

                return Task.CompletedTask;
            }, "superglue.OutputSetup");
        }
    }
}