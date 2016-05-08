using System.IO;
using Newtonsoft.Json;

namespace SuperGlue.Cms.Rendering
{
    public class RenderJsonRenderInformation : ICmsRenderer<JsonRenderInformation>
    {
        public void Render(JsonRenderInformation information, ICmsContext context, TextWriter renderTo)
        {
            var json = JsonConvert.SerializeObject(information.Data);
            renderTo.Write(json);
        }
    }
}