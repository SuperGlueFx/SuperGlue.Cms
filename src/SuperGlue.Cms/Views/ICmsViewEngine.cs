using System.Collections.Generic;

namespace SuperGlue.Cms.Views
{
    public interface ICmsViewEngine
    {
        CmsView FindView<TModel>(string viewName, TModel model, IEnumerable<RequestContext> contexts, bool useMaster) where TModel : class;
    }
}