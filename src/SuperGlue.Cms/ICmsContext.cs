using System;
using System.Collections.Generic;

namespace SuperGlue.Cms
{
    public interface ICmsContext
    {
        IEnumerable<RequestContext> FindCurrentContexts();
        RequestContext FindContext(string name);
        RequestContext FirstContextOf(string name, Func<RequestContext, bool> filter = null);
        RequestContext LastContextOf(string name, Func<RequestContext, bool> filter = null);
        IEnumerable<RequestContext> FindContexts(string name, Func<RequestContext, bool> filter = null);

        bool HasContext(string name);
        IDisposable EnterContexts(params RequestContext[] contexts);

        IEnumerable<TInput> Filter<TInput>(IEnumerable<TInput> input);
        bool CanRender(object input);
    }
}