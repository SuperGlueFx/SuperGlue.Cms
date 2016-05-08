using System.Collections.Generic;
using SuperGlue.Cms.Templates;

namespace SuperGlue.Cms.Rendering
{
    public class TemplateRenderInformation : IRenderInformation
    {
        public TemplateRenderInformation(IEnumerable<RequestContext> contexts, CmsTemplate template, string contentType, IDictionary<string, object> overrideSettings = null)
        {
            Contexts = contexts;
            Template = template;
            ContentType = contentType;
            OverrideSettings = overrideSettings ?? new Dictionary<string, object>();
        }

        public string ContentType { get; private set; }
        public IEnumerable<RequestContext> Contexts { get; private set; }
        public CmsTemplate Template { get; private set; }
        public IDictionary<string, object> OverrideSettings { get; private set; }
    }
}