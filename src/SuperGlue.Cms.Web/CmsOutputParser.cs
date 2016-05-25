using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using SuperGlue.Cms.Rendering;
using SuperGlue.Configuration;
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
            var shouldTransform = await (environment.GetSettings<CmsParserSettings>().ShouldTransform ?? ((x, y) => Task.FromResult(true)))(result, environment).ConfigureAwait(false);

            if (!shouldTransform)
            {
                environment.Log("Skipping transformation", LogLevel.Debug);
                return result;
            }

            using (var reader = new StreamReader(result.Body))
            {
                result.Body.Position = 0;
                var content = await reader.ReadToEndAsync().ConfigureAwait(false);

                var compiled = await _cmsRenderer.Compile(content, environment).ConfigureAwait(false);
                environment.Log("Cms content compiled", LogLevel.Debug);
                var rendered = await _cmsRenderer.Render(compiled, environment).ConfigureAwait(false);
                environment.Log("Cms content rendered", LogLevel.Debug);

                return new OutputRenderingResult(await rendered.ToStream().ConfigureAwait(false), result.ContentType);
            }
        }
    }
}