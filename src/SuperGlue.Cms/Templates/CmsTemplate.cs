namespace SuperGlue.Cms.Templates
{
    public class CmsTemplate
    {
        public CmsTemplate(string name, string body)
        {
            Name = name;
            Body = body;
        }

        public string Name { get; private set; }
        public string Body { get; private set; }
    }
}