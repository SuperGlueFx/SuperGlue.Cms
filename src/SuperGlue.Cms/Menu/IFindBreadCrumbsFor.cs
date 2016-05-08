using System.Collections.Generic;

namespace SuperGlue.Cms.Menu
{
    public interface IFindBreadCrumbsFor
    {
        IEnumerable<BreadCrumb> Get(object input);
    }
}