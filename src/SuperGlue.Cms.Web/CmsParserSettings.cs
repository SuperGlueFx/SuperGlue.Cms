using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SuperGlue.Web.Output;

namespace SuperGlue.Cms.Web
{
    public class CmsParserSettings
    {
        public Func<OutputRenderingResult, IDictionary<string, object>, Task<bool>> ShouldTransform { get; private set; }

        public void DetermineWhatToTransform(Func<OutputRenderingResult, IDictionary<string, object>, Task<bool>> shouldTransform)
        {
            ShouldTransform = shouldTransform;
        }
    }
}