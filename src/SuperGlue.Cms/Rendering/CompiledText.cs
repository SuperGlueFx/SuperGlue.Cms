using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace SuperGlue.Cms.Rendering
{
    public class CompiledText
    {
        public CompiledText(string body, IReadOnlyDictionary<string, DataSource> dataSources = null)
        {
            Body = body;
            DataSources = dataSources ?? new ReadOnlyDictionary<string, DataSource>(new Dictionary<string, DataSource>());
        }

        public string Body { get; private set; }
        public IReadOnlyDictionary<string, DataSource> DataSources { get; private set; }

        public class DataSource
        {
            public DataSource(string type, IReadOnlyDictionary<string, object> settings)
            {
                Type = type;
                Settings = settings;
            }
            
            public string Type { get; private set; }
            public IReadOnlyDictionary<string, object> Settings { get; private set; }
        }
    }
}