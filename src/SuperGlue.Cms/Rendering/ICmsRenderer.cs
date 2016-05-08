using System.Collections.Generic;
using System.IO;
using SuperGlue.Cms.Components;
using SuperGlue.Cms.Templates;

namespace SuperGlue.Cms.Rendering
{
    public interface ICmsRenderer
    {
        IRenderResult RenderComponent(ICmsComponent component, IDictionary<string, object> settings, ICmsContext context);
        IRenderResult RenderTemplate(CmsTemplate template, IDictionary<string, object> settings, ICmsContext context);
        IRenderResult ParseText(string text, ICmsContext context, ParseTextOptions options = null);
    }

    public interface ICmsRenderer<in TRenderInformation> where TRenderInformation : IRenderInformation
    {
        void Render(TRenderInformation information, ICmsContext context, TextWriter renderTo);
    }
}