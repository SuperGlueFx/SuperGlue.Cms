using System.Collections.Generic;

namespace SuperGlue.Cms.Rendering
{
    public interface IRenderInformation
    {
        string ContentType { get; }
        IEnumerable<RequestContext> Contexts { get; }
    }
}