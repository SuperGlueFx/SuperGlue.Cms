using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace SuperGlue.Cms
{
    public class RequestContext
    {
        public RequestContext(string name, IDictionary<string, object> data)
        {
            Name = name;
            Data = new ReadOnlyDictionary<string, object>(data);
        }

        public string Name { get; private set; }
        public IReadOnlyDictionary<string, object> Data { get; private set; }
    }
}