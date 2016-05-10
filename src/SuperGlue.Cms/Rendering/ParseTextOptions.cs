namespace SuperGlue.Cms.Rendering
{
    public class ParseTextOptions
    {
        public ParseTextOptions(bool filterOnlyFirstLevel)
        {
            FilterOnlyFirstLevel = filterOnlyFirstLevel;
        }
        
        public bool FilterOnlyFirstLevel { get; private set; }
    }
}