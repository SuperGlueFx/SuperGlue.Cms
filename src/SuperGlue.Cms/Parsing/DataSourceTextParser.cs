using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SuperGlue.Cms.Rendering;

namespace SuperGlue.Cms.Parsing
{
    public class DataSourceTextParser : RegexTextParser
    {
        protected override Task<CompiledText> CompileInner(Match match, IDictionary<string, object> environment, Func<string, Task<string>> recurse)
        {
            var type = match.Groups["dataSourceType"].Value;
            var name = match.Groups["dataSourceName"].Value;
            var settings = JsonConvert.DeserializeObject<IDictionary<string, object>>(match.Groups["settings"].Value);

            return Task.FromResult(new CompiledText("", new ReadOnlyDictionary<string, CompiledText.DataSource>(new Dictionary<string, CompiledText.DataSource>
            {
                [name] = new CompiledText.DataSource(type, new ReadOnlyDictionary<string, object>(settings))
            })));
        }

        protected override IEnumerable<Regex> GetRegexes()
        {
            yield return new Regex(@"\!\[DataSource\.((?<dataSourceType>.[a-z&auml;&aring;&ouml;A-Z&Auml;&Aring;&Ouml;0-9.\-]*))\.((?<dataSourceName>.[a-z&auml;&aring;&ouml;A-Z&Auml;&Aring;&Ouml;0-9.\-]*)) settings\=((?<settings>[a-z&auml;&aring;&ouml;A-Z&Auml;&Aring;&Ouml;0-9\:\,\{\}""\/\.\-\=]*))\]\!", RegexOptions.Compiled);
        }
    }
}