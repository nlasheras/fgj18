
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScreen : MonoBehaviour {

    private const string startLevel = "lvl1AttackJumpMove";

	// Update is called once per frame
	void Update () {
        if ( Input.GetKeyDown ( KeyCode.Space ) )
        {
            SceneManager.LoadScene ( startLevel, LoadSceneMode.Single );
        }
    }
}
