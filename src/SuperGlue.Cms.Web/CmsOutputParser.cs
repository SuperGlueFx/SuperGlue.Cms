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
            var parsed = await _cmsRenderer.ParseText(result.Body, environment).ConfigureAwait(false);

            return new OutputRenderingResult(parsed, result.ContentType);
        }
    }
}