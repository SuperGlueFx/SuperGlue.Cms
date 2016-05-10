using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SuperGlue.Cms.Components;
using SuperGlue.Cms.Rendering;

namespace SuperGlue.Cms.Parsing
{
    public class ComponentTextParser : RegexTextParser
    {
        private readonly IEnumerable<ICmsComponent> _components;

        public ComponentTextParser(IEnumerable<ICmsComponent> components)
        {
            _components = components;
        }

        protected override async Task<object> FindParameterValue(Match match, ICmsRenderer cmsRenderer, Func<string, Task<string>> recurse)
        {
            var componentNameGroup = match.Groups["componentName"];

            if (componentNameGroup == null)
                return "";

            var componentName = componentNameGroup.Value;

            var component = _components.FirstOrDefault(x => x.GetType().Name == componentName);

            if (component == null)
                return "";

            var settings = component.GetDefaultSettings();

            var settingsGroup = match.Groups["settings"];

            if (!string.IsNullOrEmpty(settingsGroup?.Value))
            {
                var settingsJson = settingsGroup.Value;

                if (!string.IsNullOrEmpty(settingsJson))
                {
                    var parsedSettings = JsonConvert.DeserializeObject<IDictionary<string, object>>(settingsJson);

                    foreach (var item in parsedSettings)
                        settings[item.Key] = item.Value;
                }
            }

            var renderResult = await cmsRenderer.RenderComponent(component, settings).ConfigureAwait(false);

            return await recurse(renderResult).ConfigureAwait(false);
        }

        protected override IEnumerable<Regex> GetRegexes()
        {
            yield return new Regex(@"\!\[Components\.((?<componentName>.[a-z&auml;&aring;&ouml;A-Z&Auml;&Aring;&Ouml;0-9.""\]\[]*))\]\!", RegexOptions.Compiled);
            yield return new Regex(@"\!\[Components\.((?<componentName>.[a-z&auml;&aring;&ouml;A-Z&Auml;&Aring;&Ouml;0-9.""\]\[]*)) Settings\=((?<settings>[a-z&auml;&aring;&ouml;A-Z&Auml;&Aring;&Ouml;0-9. \-\:\,""\]\[\{\}]*))\]\!",
                RegexOptions.Compiled);
        }
    }
}