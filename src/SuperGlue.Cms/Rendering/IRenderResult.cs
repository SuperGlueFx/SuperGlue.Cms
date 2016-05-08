using System.IO;

namespace SuperGlue.Cms.Rendering
{
    public interface IRenderResult
    {
        string ContentType { get; }
        void RenderTo(TextWriter writer);
        string Read();
    }
}