using System.Collections.Generic;

namespace SuperGlue.Cms.Files
{
    public interface ITransformFiles
    {
        string Name { get; }
        IEnumerable<ITransformationSetting> GetTransformationSettings();
    }
}