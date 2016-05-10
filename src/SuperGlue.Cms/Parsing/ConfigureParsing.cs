using System.Collections.Generic;
using System.Threading.Tasks;
using SuperGlue.Configuration;

namespace SuperGlue.Cms.Parsing
{
    public class ConfigureParsing : ISetupConfigurations
    {
        public IEnumerable<ConfigurationSetupResult> Setup(string applicationEnvironment)
        {
            yield return new ConfigurationSetupResult("superglue.Cms.ParsingSetup", environment =>
            {
                environment.RegisterAll(typeof(IParseModelExpression));
                environment.RegisterAll(typeof(ITextParser));
                environment.RegisterTransient(typeof(IFindParameterValueFromModel), typeof(DefaultParameterValueFinder));

                return Task.CompletedTask;
            }, "superglue.ContainerSetup");
        }
    }
}