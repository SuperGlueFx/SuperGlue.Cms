using System.Collections.Generic;
using System.Threading.Tasks;

namespace SuperGlue.Cms.Rendering
{
    public interface IExecuteDataSource
    {
        string Type { get; }
        Task<dynamic> Execute(IReadOnlyDictionary<string, object> settings);
    }
}