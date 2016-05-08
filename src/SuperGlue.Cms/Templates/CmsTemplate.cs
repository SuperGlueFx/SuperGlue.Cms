using System.Collections.Generic;

namespace SuperGlue.Cms.Templates
{
    public class CmsTemplate : IRequireContexts
    {
        private readonly IEnumerable<string> _requiredContexts;

        public CmsTemplate(string name, string body, string contentType, params string[] requiredContexts)
        {
            _requiredContexts = requiredContexts;
            Name = name;
            Body = body;
            ContentType = contentType;
        }

        public string Name { get; private set; }
        public string Body { get; private set; }
        public string ContentType { get; private set; }
        
        public IEnumerable<string> GetRequiredContexts()
        {
            return _requiredContexts;
        }
    }
}