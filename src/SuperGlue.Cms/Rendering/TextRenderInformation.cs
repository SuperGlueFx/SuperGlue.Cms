using System.Collections.Generic;

namespace SuperGlue.Cms.Rendering
{
    public class TextRenderInformation : IRenderInformation
    {
        public TextRenderInformation(IEnumerable<RequestContext> contexts, string text, string contentType)
        {
            Contexts = contexts;
            Text = text;
            ContentType = contentType;
        }

        public string ContentType { get; }
        public IEnumerable<RequestContext> Contexts { get; }
        public string Text { get; private set; }
    }
}