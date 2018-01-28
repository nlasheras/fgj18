using UnityEngine;

public class EffectsManager : SingletonBehaviour<EffectsManager>
{

    private ScreenShaker screenShaker;
    private AudioSource audioSource;

    [SerializeField]
    private AudioClip jumpSFX;

    [SerializeField]
    private AudioClip headButtSFX;

    [SerializeField]
    private AudioClip deathSFX;

    private void Awake ()
    {
        RegisterSingleton ();
        screenShaker = GetComponent<ScreenShaker> ();
        audioSource = GetComponent<AudioSource> ();
    }

    void OnDestroy ()
    {
        UnregisterSingleton ();
    }

    public void RandomShake ()
    {
        screenShaker.Shake ( Random.Range ( -1f, 1f ), Random.Range ( -1f, 1f ) );
    }

    public void JumpEffects ()
    {
        if ( audioSource != null )
        {
            audioSource.PlayOneShot ( jumpSFX );
        }
    }

    public void HeadButtEffects ()
    {
        if ( audioSource != null )
        {
            audioSource.PlayOneShot ( headButtSFX );
        }

        RandomShake ();
    }

    public void DeathEffects ()
    {
        if ( audioSource != null )
        {
            audioSource.PlayOneShot ( deathSFX );
        }
    }
}
