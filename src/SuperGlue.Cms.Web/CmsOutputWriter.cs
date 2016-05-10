using System.Collections.Generic;
using System.Threading.Tasks;
using SuperGlue.Cms.Rendering;
using SuperGlue.Web;
using SuperGlue.Web.Output;

namespace SuperGlue.Cms.Web
{
    public class CmsOutputWriter : IWriteToOutput
    {
        private readonly ICmsRenderer _cmsRenderer;

        public CmsOutputWriter(ICmsRenderer cmsRenderer)
        {
            _cmsRenderer = cmsRenderer;
        }

        public async Task Write(IDictionary<string, object> environment, OutputRenderingResult result)
        {
            if (result == null)
                return;

            var response = environment.GetResponse();

            response.Headers.ContentType = result.ContentType;

            await response.Write(await _cmsRenderer.ParseText(result.Body).ConfigureAwait(false)).ConfigureAwait(false);
        }
    }
}