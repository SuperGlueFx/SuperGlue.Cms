using System.Collections.Generic;
using System.Threading.Tasks;
using SuperGlue.Cms.Components;
using SuperGlue.Cms.Templates;

namespace SuperGlue.Cms.Rendering
{
    public interface ICmsRenderer
    {
        Task<string> RenderComponent(ICmsComponent component, IDictionary<string, object> settings);
        Task<string> RenderTemplate(CmsTemplate template, IDictionary<string, object> settings);
        Task<string> ParseText(string text, ParseTextOptions options = null);
    }
}