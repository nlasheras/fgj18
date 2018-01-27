using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class MusicManager : SingletonBehaviour<MusicManager> {

    private AudioSource musicSource;

    [SerializeField]
    private AudioClip themeMusic;
    [SerializeField]
    private float startDelay = 0.5f;
    [SerializeField]
    protected float fadeRate = 2f;

    private float originalVolume;
    private float fadeLevel = 1f;

    private void Awake ()
    {
        RegisterSingleton ();
        musicSource = GetComponent<AudioSource> ();
    }

    protected void Start ()
    {
        originalVolume = musicSource.volume;

        PlayMusic ( themeMusic );
    }

    private void PlayMusic ( AudioClip music, bool fadeIn = false, bool loop = true )
    {
        musicSource.Stop ();

        musicSource.loop = loop;
        musicSource.clip = music;
        musicSource.PlayDelayed ( startDelay );

        if ( fadeIn )
        {
            fadeRate = -fadeRate;
        }
    }

    public void StopMusic ()
    {
        musicSource.Stop ();
    }

    public void PlayCurrentMusic ()
    {
        musicSource.Play ();
    }

    private void OnDestroy () {
        UnregisterSingleton ();
	}
}
