using System.Collections.Generic;

namespace SuperGlue.Cms.Rendering
{
    public class ParseTextOptions
    {
        public ParseTextOptions(IEnumerable<string> useOnlyParsersTagged, bool filterOnlyFirstLevel)
        {
            UseOnlyParsersTagged = useOnlyParsersTagged;
            FilterOnlyFirstLevel = filterOnlyFirstLevel;
        }

        public IEnumerable<string> UseOnlyParsersTagged { get; private set; }
        public bool FilterOnlyFirstLevel { get; private set; }
    }
}