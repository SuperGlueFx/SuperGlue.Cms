namespace SuperGlue.Cms.Features
{
    public interface IFeatureValidator
    {
        bool IsActive(string feature);
        void Load();
    }
}