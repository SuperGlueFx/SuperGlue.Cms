using System.IO;

namespace SuperGlue.Cms.Rendering
{
    public class RenderTextRenderInformation : ICmsRenderer<TextRenderInformation>
    {
        private readonly ICmsRenderer _cmsRenderer;

        public RenderTextRenderInformation(ICmsRenderer cmsRenderer)
        {
            _cmsRenderer = cmsRenderer;
        }

        public void Render(TextRenderInformation information, ICmsContext context, TextWriter renderTo)
        {
            var result = _cmsRenderer.ParseText(information.Text, context);
            result.RenderTo(renderTo);
        }
    }
}