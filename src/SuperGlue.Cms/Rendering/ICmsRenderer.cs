using System.Collections.Generic;
using SuperGlue.Cms.Components;
using SuperGlue.Cms.Templates;

namespace SuperGlue.Cms.Rendering
{
    public interface ICmsRenderer
    {
        string RenderComponent(ICmsComponent component, IDictionary<string, object> settings, ICmsContext context);
        string RenderTemplate(CmsTemplate template, IDictionary<string, object> settings, ICmsContext context);
        string ParseText(string text, ICmsContext context, ParseTextOptions options = null);
    }
}