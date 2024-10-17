public abstract class CoreComponent
{
    private bool _isInited = false;

    public void Init()
    {
        if (_isInited)
            return;

        InitOptions();
        _isInited = true;
    }

    protected abstract void InitOptions();
}
