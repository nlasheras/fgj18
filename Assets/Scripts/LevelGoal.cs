using UnityEngine;

public class LevelGoal : MonoBehaviour {
    AudioSource audioSource;
    public ParticleSystem victoryParticleSystem;

    void Start() {
        audioSource = GetComponent<AudioSource>();
    }
    void OnTriggerEnter2D(Collider2D other) {
        if ( !other.gameObject.CompareTag ( "Player" ) )
            return;

        FindObjectOfType<GameManager>().GoalReached(this);
        if (audioSource != null) audioSource.Play();
        victoryParticleSystem.Play();
    }

}
