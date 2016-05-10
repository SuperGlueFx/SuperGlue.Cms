using System.Collections.Generic;
using SuperGlue.Cms.Components;
using SuperGlue.Cms.Templates;

namespace SuperGlue.Cms.Rendering
{
    public interface ICmsRenderer
    {
        string RenderComponent(ICmsComponent component, IDictionary<string, object> settings);
        string RenderTemplate(CmsTemplate template, IDictionary<string, object> settings);
        string ParseText(string text, ParseTextOptions options = null);
    }
}