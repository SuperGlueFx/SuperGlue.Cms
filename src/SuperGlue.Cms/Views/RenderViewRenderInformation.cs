using System.Collections.Generic;
using System.IO;
using System.Linq;
using SuperGlue.Cms.Rendering;

namespace SuperGlue.Cms.Views
{
    public class RenderViewRenderInformation : ICmsRenderer<ViewRenderInformation>
    {
        private readonly IEnumerable<ICmsViewEngine> _viewEngines;

        public RenderViewRenderInformation(IEnumerable<ICmsViewEngine> viewEngines)
        {
            _viewEngines = viewEngines;
        }

        public void Render(ViewRenderInformation information, ICmsContext context, TextWriter renderTo)
        {
            var view = _viewEngines
                .Select(x => FindViewFrom(x, information.ViewName, information.Model, information.Contexts, information.UseLayout))
                .FirstOrDefault(x => x != null);

            view?.Render(renderTo, information.ContentType);
        }

        private static CmsView FindViewFrom(ICmsViewEngine viewEngine, string viewName, object model, IEnumerable<RequestContext> contexts, bool useMaster)
        {
            var findViewMethod = viewEngine
                .GetType()
                .GetMethod("FindView");

            var genericFindViewMethod = findViewMethod.MakeGenericMethod(model.GetType());

            return (CmsView)genericFindViewMethod.Invoke(viewEngine, new[] { viewName, model, contexts, useMaster });
        }
    }
}