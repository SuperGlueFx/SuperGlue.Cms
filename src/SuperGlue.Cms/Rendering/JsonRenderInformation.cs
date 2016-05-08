using System.Collections.Generic;

namespace SuperGlue.Cms.Rendering
{
    public class JsonRenderInformation : IRenderInformation
    {
        public JsonRenderInformation(IEnumerable<RequestContext> contexts, object data)
        {
            Contexts = contexts;
            Data = data;
        }

        public string ContentType { get { return "Application/Json"; } }
        public object Data { get; private set; }
        public IEnumerable<RequestContext> Contexts { get; private set; }
    }
}