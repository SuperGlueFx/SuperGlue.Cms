using System.Threading.Tasks;

namespace SuperGlue.Cms.Templates
{
    public interface ITemplateStorage
    {
        Task<CmsTemplate> Load(string name);
    }

    public class FileSystemTemplateStorage : ITemplateStorage
    {
        public Task<CmsTemplate> Load(string name)
        {
            return null;
        }
    }
}