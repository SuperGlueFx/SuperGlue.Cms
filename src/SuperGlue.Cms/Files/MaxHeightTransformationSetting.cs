namespace SuperGlue.Cms.Files
{
    public class MaxHeightTransformationSetting : ITransformationSetting
    {
        public MaxHeightTransformationSetting(int maxHeight)
        {
            MaxHeight = maxHeight;
        }

        public int MaxHeight { get; private set; }
    }
}