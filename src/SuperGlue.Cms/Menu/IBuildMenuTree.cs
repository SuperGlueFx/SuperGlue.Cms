using System.Collections.Generic;

namespace SuperGlue.Cms.Menu
{
    public interface IBuildMenuTree
    {
        int Order { get; }
        string ForMenu { get; }
        IEnumerable<MenuItemSettings> Build(object currentItem);
    }
}