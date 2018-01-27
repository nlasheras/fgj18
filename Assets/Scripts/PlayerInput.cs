﻿using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent ( typeof ( Player ) )]
public class PlayerInput : MonoBehaviour {

    Player player;

	// Use this for initialization
	void Start () {
        player = GetComponent<Player> ();
	}
	
	// Update is called once per frame
	void Update () {
       Vector2 directionalInput = new Vector2 ( Input.GetAxisRaw ( "Horizontal" ), Input.GetAxisRaw ( "Vertical" ) );
        player.SetDirectionalInput ( directionalInput );

        if ( Input.GetKeyDown ( KeyCode.Space ) )
        {
            player.OnJumpInputDown ();
        }
        if ( Input.GetKeyUp ( KeyCode.Space ) )
        {
            player.OnJumpInputUp ();
        }
        if ( Input.GetKeyDown ( KeyCode.R ) )
        {
            SceneManager.LoadScene ( SceneManager.GetActiveScene ().name, LoadSceneMode.Single );
        }
        if (Input.GetKeyUp(KeyCode.F))
        {
            player.OnAttackInputUp();
        }
    }
}
