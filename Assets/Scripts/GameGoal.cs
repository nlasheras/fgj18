using UnityEngine;

public class GameGoal : MonoBehaviour
{
    AudioSource audioSource;
    public ParticleSystem victoryParticleSystem;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.gameObject.CompareTag("Player"))
            return;

        GameManager.Instance.GameEndReached();

        if (audioSource != null)
            audioSource.Play();

        victoryParticleSystem.Play();
    }

}