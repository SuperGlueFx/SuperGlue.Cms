using System.Collections.Generic;

namespace SuperGlue.Cms.Features
{
    public interface IBelongToFeatures
    {
        IEnumerable<string> GetFeatures();
    }
}