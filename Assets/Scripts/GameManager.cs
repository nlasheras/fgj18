using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    public GameObject playerPrefab;
    [HideInInspector]
	public Vector2 playerRespawnPoint;

    float levelStartedAt;

    enum GameState { NotStarted, Playing, Done }
    GameState gameState = GameState.NotStarted;

    public bool canJump = true;
    public bool canMoveBack = true;
    public bool canAttack = true;
    public bool canWallSlide = true;

    int currentLevel = 0;

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("Scene loaded");
        StartGame();
    }

    public void Awake ()
    {
        GameObject.DontDestroyOnLoad(gameObject);
        if (FindObjectsOfType(GetType()).Length > 1)
        {
            Destroy(gameObject);
        }
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
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

    public void GoalReached(LevelGoal goal, Player.PlayerSkill playerSkill) {

        switch (playerSkill)
        {
            case Player.PlayerSkill.ATTACK:
                canAttack = false;
                break;
            case Player.PlayerSkill.JUMP:
                canJump = false;
                break;
            case Player.PlayerSkill.MOVE_BACK:
                canMoveBack = false;
                break;
            case Player.PlayerSkill.WALL_SLIDE:
                canWallSlide = false;
                break;
            default:
                break;
        }
        

        if (gameState != GameState.Playing) return;
        gameState = GameState.Done;
        float totalTime = Time.time - levelStartedAt;

        currentLevel++;
        SceneManager.LoadScene(currentLevel);
    }
}
