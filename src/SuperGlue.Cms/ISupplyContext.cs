using System.Collections.Generic;

namespace SuperGlue.Cms
{
    public interface ISupplyContext
    {
        IEnumerable<RequestContext> GetContexts();
    }
}