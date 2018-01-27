using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public GameObject playerPrefab;
    [HideInInspector]
	public Vector2 playerRespawnPoint;

    float levelStartedAt;

    enum GameState { NotStarted, Playing, Done }
    GameState gameState = GameState.NotStarted;


    public void Start ()
    {
        StartGame ();
    }

    public void PlayerDied() {
        StartCoroutine(RespawnPlayer());
    }

    IEnumerator RespawnPlayer() {
        yield return new WaitForSeconds(.5f);
        MakePlayer();
    }

    void MakePlayer() {
        var startPoint = FindObjectOfType<LevelStart>();
        if (startPoint != null) {
            playerRespawnPoint = startPoint.transform.position;
            startPoint.LevelStarted();
        }
        Instantiate(playerPrefab, playerRespawnPoint, playerPrefab.transform.rotation);
    }

    public void StartGame() {
        gameState = GameState.Playing;
        MakePlayer();
        levelStartedAt = Time.time;
    }

    public void GoalReached(LevelGoal goal) {
        if (gameState != GameState.Playing) return;
        gameState = GameState.Done;
        float totalTime = Time.time - levelStartedAt;
    }
}
