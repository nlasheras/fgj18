using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectsManager : SingletonBehaviour<EffectsManager> {

    private ScreenShaker screenShaker;

    private void Awake ()
    {
        RegisterSingleton ();
        screenShaker = GetComponent<ScreenShaker> ();
    }

    void OnDestroy ()
    {
        UnregisterSingleton ();
    }

    public void RandomShake ()
    {
        screenShaker.Shake ( Random.Range(-1f, 1f), Random.Range ( -1f, 1f ) );
    }
}
