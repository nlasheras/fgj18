public class SoundManager : SingletonBehaviour<SoundManager>
{
    private void Awake ()
    {
        RegisterSingleton ();
    }

    private void OnDestroy ()
    {
        UnregisterSingleton ();
    }
}
