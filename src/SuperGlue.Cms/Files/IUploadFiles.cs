namespace SuperGlue.Cms.Files
{
    public interface IUploadFiles
    {
        string Upload(UploadFile file);
        string GetPath(string file, ITransformFiles transformer = null);
        bool Exists(string file);
    }
}