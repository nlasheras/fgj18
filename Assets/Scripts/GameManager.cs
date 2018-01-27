using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : SingletonBehaviour<GameManager> {

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

    private Player player;

    int currentLevel = 0;

    private void Awake ()
    {
        RegisterSingleton ();
        currentLevel = SceneManager.GetActiveScene ().buildIndex;
    }

    private void Update ()
    {
        switch ( gameState )
        {
            case GameState.NotStarted:
                if ( Input.GetKeyDown ( KeyCode.Space ) )
                {
                    StartGame ();
                }
                break;
            case GameState.Playing:
                if ( Input.GetKeyDown ( KeyCode.R ) )
                {
                    SceneManager.LoadScene ( SceneManager.GetActiveScene ().name, LoadSceneMode.Single );
                }
                break;
            case GameState.Done:
                break;
            default:
                break;
        }
    }

    public void StartLevel()
    {
        gameState = GameState.NotStarted;
        Debug.Log ( "currentLevel:" + currentLevel );
        SceneManager.LoadScene ( currentLevel, LoadSceneMode.Single );
    }

    public void StartGame ()
    {
        gameState = GameState.Playing;
        SpawnPlayer ();
        levelStartedAt = Time.time;
    }

    public void PlayerDied ()
    {
        gameState = GameState.NotStarted;
        Destroy ( player.gameObject );
        player = null;
    }

    void SpawnPlayer()
    {
        var startPoint = FindObjectOfType<LevelStart>();

        if (startPoint != null)
        {
            playerRespawnPoint = startPoint.transform.position;
            startPoint.LevelStarted();
        }

        player = Instantiate(playerPrefab, playerRespawnPoint, playerPrefab.transform.rotation).GetComponent<Player>();
    }

    public void GoalReached(LevelGoal goal, Player.PlayerSkill playerSkill)
    {
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
        Debug.Log ( "currentLevel:" +currentLevel );
        StartLevel ();
    }

    private void OnDestroy ()
    {
        Debug.Log ( "GameManager OnDestroy" );
        UnregisterSingleton ();
    }
}
