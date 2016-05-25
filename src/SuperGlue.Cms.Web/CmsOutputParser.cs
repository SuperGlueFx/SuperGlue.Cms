using System.Collections.Generic;
using System.IO;
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
            using (var reader = new StreamReader(result.Body))
            {
                result.Body.Position = 0;
                var content = await reader.ReadToEndAsync().ConfigureAwait(false);

                var compiled = await _cmsRenderer.Compile(content, environment).ConfigureAwait(false);
                var rendered = await _cmsRenderer.Render(compiled, environment).ConfigureAwait(false);

                return new OutputRenderingResult(await rendered.ToStream().ConfigureAwait(false), result.ContentType);
            }
        }
    }
}