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

        public Task Write(IDictionary<string, object> environment, OutputRenderingResult result)
        {
            if (result == null)
                return Task.CompletedTask;

            var response = environment.GetResponse();

            response.Headers.ContentType = result.ContentType;

            return response.Write(_cmsRenderer.ParseText(result.Body));
        }
    }
}