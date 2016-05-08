using System.Collections.Generic;

namespace SuperGlue.Cms
{
    public interface IRequireContexts
    {
        IEnumerable<string> GetRequiredContexts();
    }
}