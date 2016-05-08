namespace SuperGlue.Cms.Menu
{
    public interface IMenuContext
    {
        T Get<T>() where T : class;
        TService Service<TService>();
    }
}