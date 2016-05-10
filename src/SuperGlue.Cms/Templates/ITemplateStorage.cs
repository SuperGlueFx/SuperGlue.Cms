using System.Threading.Tasks;

namespace SuperGlue.Cms.Templates
{
    public interface ITemplateStorage
    {
        Task<CmsTemplate> Load(string name);
    }
}