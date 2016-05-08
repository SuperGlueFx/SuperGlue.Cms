namespace SuperGlue.Cms.Templates
{
    public interface ITemplateStorage
    {
        CmsTemplate Load(string name);
    }
}