using System;

namespace SuperGlue.Cms
{
    public static class CmsContextExtensions
    {
        public static TResult GetDataFromContext<TResult>(this ICmsContext cmsContext, string name, Func<RequestContext, TResult> getResult)
        {
            var context = cmsContext.LastContextOf(name);

            return context == null ? default(TResult) : getResult(context);
        }
    }
}