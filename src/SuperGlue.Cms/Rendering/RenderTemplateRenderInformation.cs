using System.IO;

namespace SuperGlue.Cms.Rendering
{
    public class RenderTemplateRenderInformation : ICmsRenderer<TemplateRenderInformation>
    {
        private readonly ICmsRenderer _cmsRenderer;

        public RenderTemplateRenderInformation(ICmsRenderer cmsRenderer)
        {
            _cmsRenderer = cmsRenderer;
        }

        public void Render(TemplateRenderInformation information, ICmsContext context, TextWriter renderTo)
        {
            var renderResult = _cmsRenderer.RenderTemplate(information.Template, information.OverrideSettings, context);
            renderResult.RenderTo(renderTo);
        }
    }
}