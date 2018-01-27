using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager> {

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
        StartGame();
    }

    private Player player;

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    public void PlayerDied() {
        SceneManager.LoadScene ( SceneManager.GetActiveScene ().name, LoadSceneMode.Single );
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
        player = Instantiate(playerPrefab, playerRespawnPoint, playerPrefab.transform.rotation).GetComponent<Player>();
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
        

        if (gameState != GameState.Playing)
            return;
        gameState = GameState.Done;
        float totalTime = Time.time - levelStartedAt;

        currentLevel++;
        SceneManager.LoadScene(currentLevel);
    }
}
