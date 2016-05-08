using System;
using System.Collections.Generic;
using System.Linq;

namespace SuperGlue.Cms.Menu
{
    public class Menu
    {
        public Menu(string name, IEnumerable<MenuItem> menuItems)
        {
            MenuItems = menuItems;
            Name = name;
        }

        public string Name { get; private set; }
        public IEnumerable<MenuItem> MenuItems { get; private set; }

        public class MenuItem
        {
            public MenuItem(string title, string icon, object linkTo, IEnumerable<MenuItem> children, bool active)
            {
                var childList = children.ToList();

                Active = active || childList.Any(x => x.Active);
                Children = childList;
                LinkTo = linkTo;
                Icon = icon;
                Title = title;
                UniqueId = Guid.NewGuid().ToString();
            }

            public string Title { get; private set; }
            public string Icon { get; private set; }
            public object LinkTo { get; private set; }
            public IEnumerable<MenuItem> Children { get; private set; }
            public bool Active { get; private set; }
            public string UniqueId { get; private set; }

            public bool HasChildren()
            {
                return Children.Any();
            }
        }
    }
}