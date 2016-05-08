using System;
using System.Collections.Generic;
using System.Linq;

namespace SuperGlue.Cms.Menu
{
    public class MenuItemSettings
    {
        private readonly Func<MenuItemSettings, object, IEnumerable<BreadCrumb>, bool> _isSelected;

        private MenuItemSettings(string title, string iconName, object input, Type inputType, IEnumerable<MenuItemSettings> children, Func<MenuItemSettings, object, IEnumerable<BreadCrumb>, bool> isSelected)
        {
            Children = children;
            _isSelected = isSelected;
            InputType = inputType;
            Input = input;
            IconName = iconName;
            Title = title;
        }

        public string Title { get; private set; }
        public string IconName { get; private set; }
        public object Input { get; private set; }
        public Type InputType { get; private set; }
        public IEnumerable<MenuItemSettings> Children { get; private set; }

        public bool IsSelected(object currentItem, IEnumerable<BreadCrumb> breadCrumbs)
        {
            return _isSelected(this, currentItem, breadCrumbs);
        }

        public static MenuItemSettings Create<TInput>(string title, string iconName, TInput input, IEnumerable<MenuItemSettings> children, Func<MenuItemSettings, object, IEnumerable<BreadCrumb>, bool> isSelected = null)
        {
            return new MenuItemSettings(title, iconName, input, typeof(TInput), children, isSelected ?? DefaultIsSelected);
        }

        private static bool DefaultIsSelected(MenuItemSettings item, object currentItem, IEnumerable<BreadCrumb> breadCrumbs)
        {
            if (currentItem == null)
                return false;

            var selected = item.InputType == currentItem.GetType();

            return selected || item.Children.Any(x => x.IsSelected(currentItem, breadCrumbs)) || breadCrumbs.Any(x => x.LinkTo != null && x.LinkTo.GetType() == item.InputType);
        }
    }
}