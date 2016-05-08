using System.Collections.Generic;
using SuperGlue.Cms.Rendering;

namespace SuperGlue.Cms.Views
{
    public class ViewRenderInformation : IRenderInformation
    {
        public ViewRenderInformation(string viewName, object model, IEnumerable<RequestContext> contexts, string contentType, bool useLayout)
        {
            ViewName = viewName;
            Model = model;
            Contexts = contexts;
            ContentType = contentType;
            UseLayout = useLayout;
        }

        public string ViewName { get; private set; }
        public object Model { get; private set; }
        public string ContentType { get; private set; }
        public IEnumerable<RequestContext> Contexts { get; private set; }
        public bool UseLayout { get; private set; }
    }
}