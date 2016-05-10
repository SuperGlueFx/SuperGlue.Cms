using System.Threading.Tasks;
using SuperGlue.FileSystem;

namespace SuperGlue.Cms.Files
{
    //HACK:Not completed
    public class FileSystemFileUploader : IUploadFiles
    {
        private readonly IFileSystem _fileSystem;

        public FileSystemFileUploader(IFileSystem fileSystem)
        {
            _fileSystem = fileSystem;
        }

        public async Task<string> Upload(UploadFile file)
        {
            await _fileSystem.WriteStreamToFile(file.Name, file.Data).ConfigureAwait(false);

            return file.Name;
        }

        public string GetPath(string file, ITransformFiles transformer = null)
        {
            return file;
        }

        public bool Exists(string file)
        {
            return _fileSystem.FileExists(file);
        }
    }
}