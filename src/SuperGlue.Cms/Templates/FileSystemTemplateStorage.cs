using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SuperGlue.Configuration;
using SuperGlue.FileSystem;

namespace SuperGlue.Cms.Templates
{
    public class FileSystemTemplateStorage : ITemplateStorage
    {
        private readonly IFileSystem _fileSystem;
        private readonly IDictionary<string, object> _environment;

        public FileSystemTemplateStorage(IFileSystem fileSystem, IDictionary<string, object> environment)
        {
            _fileSystem = fileSystem;
            _environment = environment;
        }

        public async Task<CmsTemplate> Load(string name)
        {
            var settings = _environment.GetSettings<FileSystemTemplatesSettings>();
            var matchingFile = _fileSystem.FindFiles(_environment.ResolvePath(settings.TemplateFolder), FileSet.Deep($"{name}.template")).FirstOrDefault();

            if (string.IsNullOrEmpty(matchingFile))
                return null;

            var content = await _fileSystem.ReadStringFromFile(matchingFile).ConfigureAwait(false);

            return new CmsTemplate(name, content);
        }
    }
}