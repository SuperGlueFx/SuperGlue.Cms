using System.Collections.Generic;
using System.Threading.Tasks;
using SuperGlue.Configuration;
using SuperGlue.Configuration.Ioc;

namespace SuperGlue.Cms.Parsing
{
    public class ConfigureParsing : ISetupConfigurations
    {
        public IEnumerable<ConfigurationSetupResult> Setup(string applicationEnvironment)
        {
            yield return new ConfigurationSetupResult("superglue.Cms.ParsingSetup", environment =>
            {
                environment.AlterSettings<IocConfiguration>(x => x.Register(typeof(IFindParameterValueFromModel), typeof(DefaultParameterValueFinder))
                   .Scan(typeof(IParseModelExpression))
                   .Scan(typeof(ITextParser)));

                return Task.CompletedTask;
            }, "superglue.ContainerSetup");
        }
    }
}