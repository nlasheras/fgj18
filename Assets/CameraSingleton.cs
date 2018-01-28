

public class CameraSingleton : SingletonBehaviour<CameraSingleton> {

    private void Awake ()
    {
        RegisterSingleton ();
    }

    private void OnDestroy ()
    {
        UnregisterSingleton ();
    }
}
