using System.Collections.Generic;
using System.Threading.Tasks;
using SuperGlue.Cms.Rendering;
using SuperGlue.Web.Output;

namespace SuperGlue.Cms.Web
{
    public class CmsOutputParser : ITransformOutput
    {
        private readonly ICmsRenderer _cmsRenderer;

        public CmsOutputParser(ICmsRenderer cmsRenderer)
        {
            _cmsRenderer = cmsRenderer;
        }

        public async Task<OutputRenderingResult> Transform(OutputRenderingResult result, IDictionary<string, object> environment)
        {
            var compiled = await _cmsRenderer.Compile(result.Body, environment).ConfigureAwait(false);
            var rendered = await _cmsRenderer.Render(compiled, environment).ConfigureAwait(false);

            return new OutputRenderingResult(rendered, result.ContentType);
        }
    }
}