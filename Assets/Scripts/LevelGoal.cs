using UnityEngine;

public class LevelGoal : MonoBehaviour {
    public Player.PlayerSkill skillToDisable;

    AudioSource audioSource;
    public ParticleSystem victoryParticleSystem;

    void Start() {
        audioSource = GetComponent<AudioSource>();
    }
    void OnTriggerEnter2D(Collider2D other) {
        if ( !other.gameObject.CompareTag ( "Player" ) )
            return;

        FindObjectOfType<GameManager>().GoalReached(this, skillToDisable);
        if (audioSource != null) audioSource.Play();
        victoryParticleSystem.Play();
    }

}
