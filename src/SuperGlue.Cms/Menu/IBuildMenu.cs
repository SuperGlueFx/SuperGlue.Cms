namespace SuperGlue.Cms.Menu
{
    public interface IBuildMenu
    {
        Menu Build(string name, object currentInput);
    }
}