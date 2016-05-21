using System.Threading.Tasks;
using SuperGlue.Cms.Templates;

namespace SuperGlue.Cms.Rendering
{
    public interface ICmsRenderer
    {
        Task<string> RenderTemplate(CmsTemplate template);
        Task<string> ParseText(string text, ParseTextOptions options = null);
    }
}