using System;
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
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded ( Scene arg0, LoadSceneMode arg1 )
    {
        if (currentLevel < 5)
            StartGame ();
    }

    private void Update ()
    {
        switch ( gameState )
        {
            case GameState.NotStarted:
                 StartGame ();
                break;
            case GameState.Playing:
                break;
            case GameState.Done:
                break;
            default:
                break;
        }
    }

    public void LoadLevel()
    {
        Debug.Log ( "currentLevel:" + currentLevel );
        SceneManager.LoadScene ( currentLevel, LoadSceneMode.Single );
        StartGame ();
    }

    public void StartGame ()
    {
        Debug.Log ( "Starting game" );
        gameState = GameState.Playing;
        SpawnPlayer ();
        levelStartedAt = Time.time;
    }

    public void PlayerDied ()
    {
        if ( player == null )
            return;
        EffectsManager.Instance.DeathEffects ();
        StartCoroutine(WaitAndDie());
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

        player.GetComponent<CharacterAnimation>().playTransmission(playerSkill);
        goal.gameObject.GetComponentInChildren<StatueAnimation>().PlayAnimation();

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

        float totalTime = Time.time - levelStartedAt;

        StartCoroutine(WaitAndStartNextLevel());

    }

    protected IEnumerator WaitAndStartNextLevel()
    {
        player.disableUpdate = true;

        currentLevel++;

        Debug.Log("currentLevel:" + currentLevel);

        yield return new WaitForSeconds(7);

        player.disableUpdate = false;
        LoadLevel ();
    }

    protected IEnumerator WaitAndDie()
    {
        player.playerDead = true;
        player.GetComponent<Collider2D> ().enabled = false;
        player.disableUpdate = true;
        yield return new WaitForSeconds(3);
        player.disableUpdate = false;
        player.playerDead = false;
        gameState = GameState.NotStarted;
        Destroy(player.gameObject);
        player = null;

    }

    public void GameEndReached()
    {
        currentLevel++;
        LoadLevel();
    }

    private void OnDestroy ()
    {
        Debug.Log ( "GameManager OnDestroy" );
        UnregisterSingleton ();
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
}
