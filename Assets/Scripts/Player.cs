﻿using UnityEngine;


[RequireComponent ( typeof ( PlayerController ) )]
public class Player : MonoBehaviour
{

    public enum PlayerSkill {
        NONE,
        JUMP,
        MOVE_BACK,
        ATTACK,
        WALL_SLIDE
    }

    public LayerMask enemyLayer;         //Layer on which collision will be checked.

    public float maxJumpHeight = 2f;
    public float minJumpHeight = 1f;
    public float timeToJumpApex = .5f;

    public Vector2 wallJumpClimb;
    public Vector2 wallJumpOff;
    public Vector2 wallLeap;

    float moveSpeed = 6f;
    float gravity;
    float minJumpVelocity;
    float maxJumpVelocity;

    public float wallSlideSpeedMax = 3f;
    public float wallStickTime = .25f;
    float timeToWallUnstick;

    // Smoothing
    float velocityXSmoothing;
    float accelerationTimeAirborne = .2f;
    float accelerationTimeGrounded = .1f;

    Vector3 velocity;
    PlayerController controller;

    Vector2 directionalInput;

    bool wallSliding;
    int wallDirX;

    float prevXdir = 1;

    CharacterAnimation characterController;

    // Use this for initialization
    void Start ()
    {
        controller = GetComponent<PlayerController> ();
        characterController = GetComponent<CharacterAnimation>();

        gravity = -( 2 * maxJumpHeight ) / Mathf.Pow ( timeToJumpApex, 2 );
        maxJumpVelocity = Mathf.Abs ( gravity * timeToJumpApex );
        minJumpVelocity = Mathf.Sqrt ( 2 * Mathf.Abs ( gravity ) * minJumpHeight );

        Debug.Log ( "Gravity: " + gravity + " JumpVelocity: " + maxJumpVelocity );

    }

    // Update is called once per frame
    void Update ()
    {
        CalculateVelocity ();

        if (GameManager.Instance.canWallSlide)
            HandleWallSliding ();

        controller.Move ( velocity * Time.deltaTime, directionalInput );

        if (velocity.y > 0 && directionalInput.x > 0)
            characterController.jumpRight();
        else if (velocity.y > 0 && directionalInput.x < 0)
            characterController.jumpLeft();
        else if (directionalInput.x > 0)
            characterController.moveRight();
        else if (directionalInput.x < 0)
            characterController.moveLeft();
        else if (prevXdir < 0)
            characterController.idleLeft();
        else
            characterController.idleRight();

        if (controller.collisionInfo.above || controller.collisionInfo.below)
        {
            velocity.y = 0;
        }
    }

    public void SetDirectionalInput ( Vector2 input )
    {
        if (GameManager.Instance.canMoveBack || input.x > 0 )
            directionalInput = input;

        if (input.x != 0)
            prevXdir = input.x;
    }

    public void OnJumpInputDown ()
    {
        if (!GameManager.Instance.canJump)
            return;

        Debug.Log ( "JumpDown" );
        if ( wallSliding )
        {
            if ( wallDirX == directionalInput.x )
            {
                velocity.x = -wallDirX * wallJumpClimb.x;
                velocity.y = wallJumpClimb.y;
            }
            else if ( directionalInput.x == 0 )
            {
                velocity.x = -wallDirX * wallJumpOff.x;
                velocity.y = wallJumpOff.y;
            }
            else if ( wallDirX == -directionalInput.x )
            {
                velocity.x = -wallDirX * wallLeap.x;
                velocity.y = wallLeap.y;
            }

        }
        if ( controller.collisionInfo.below )
        {
            velocity.y = maxJumpVelocity;
        }
    }

    public void OnJumpInputUp ()
    {
        Debug.Log ( "JumpUp" );
        if (GameManager.Instance.canJump && velocity.y > minJumpVelocity )
        {
            velocity.y = minJumpVelocity;
        }
    }

    public void OnAttackInputUp()
    {
        Debug.Log("attack");
        if (GameManager.Instance.canAttack)
        {
            EffectsManager.Instance.RandomShake ();
            Vector2 end = transform.position;
            if (directionalInput.x >= 0)
            {
                characterController.attackRight();
                Debug.Log("attack right");
                end.x += 2;
            }
                
            else if (directionalInput.x < 0)
            {
                characterController.attackLeft();
                Debug.Log("attack left");
                end.x -= 2;
            }
            RaycastHit2D hit;
            hit = Physics2D.Linecast(transform.position, end, enemyLayer);

            if (hit && hit.transform.CompareTag("enemy"))
            {
                Debug.Log("Player damaged ENEMY");
                Destroy(hit.transform.gameObject);
            }
        }
    }

    private void OnDrawGizmos()
    {
    }

    void HandleWallSliding ()
    {
        wallDirX = ( controller.collisionInfo.left ) ? -1 : 1;
        wallSliding = false;

        if ( ( controller.collisionInfo.left || controller.collisionInfo.right ) && !controller.collisionInfo.below && velocity.y < 0 )
        {
            wallSliding = true;

            if ( velocity.y < -wallSlideSpeedMax )
            {
                velocity.y = -wallSlideSpeedMax;
            }

            if ( timeToWallUnstick > 0 )
            {
                velocityXSmoothing = 0f;
                velocity.x = 0f;

                if ( directionalInput.x != wallDirX && directionalInput.x != 0 )
                {
                    timeToWallUnstick -= Time.deltaTime;
                }
                else
                {
                    timeToWallUnstick = wallStickTime;
                }
            }
            else
            {
                timeToWallUnstick = wallStickTime;
            }
        }
    }

    void CalculateVelocity ()
    {
        float targetVelocityX = directionalInput.x * moveSpeed;
        velocity.x = Mathf.SmoothDamp ( velocity.x, targetVelocityX, ref velocityXSmoothing, ( controller.collisionInfo.below ) ? accelerationTimeGrounded : accelerationTimeAirborne );
        velocity.y += gravity * Time.deltaTime;
    }
}
