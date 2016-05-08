namespace SuperGlue.Cms.Menu
{
    public class BreadCrumb
    {
        public BreadCrumb(object linkTo, string linkText, string icon)
        {
            Icon = icon;
            LinkText = linkText;
            LinkTo = linkTo;
        }

        public object LinkTo { get; private set; }
        public string LinkText { get; private set; }
        public string Icon { get; private set; }
    }
}