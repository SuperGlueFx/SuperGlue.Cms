namespace SuperGlue.Cms.Files
{
    public class MaxWidthTransformationSetting : ITransformationSetting
    {
        public MaxWidthTransformationSetting(int maxWidth)
        {
            MaxWidth = maxWidth;
        }

        public int MaxWidth { get; private set; }
    }
}