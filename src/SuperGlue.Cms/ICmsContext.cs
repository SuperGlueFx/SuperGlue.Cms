using System.Collections.Generic;

namespace SuperGlue.Cms
{
    public interface ICmsContext
    {
        IEnumerable<TInput> Filter<TInput>(IEnumerable<TInput> input);
        bool CanRender(object input);
    }
}