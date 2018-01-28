using UnityEngine;

public class ScreenShaker : MonoBehaviour
{
    private Vector2 velocity;
    private Vector2 position;
    private Camera target;
    private float drag = .1f;
    private float elasticity = .5f;
    private float EFFECT_SCREEN_SHAKE_POWER = 0.5f;
    
    // Use this for initialization
    void Start ()
    {
        target = Camera.main;
        Debug.Log ( target );
    }

    public void Shake ( float powerX, float powerY )
    {
        Debug.Log ( "Shaky shake" );
        velocity.x += powerX * EFFECT_SCREEN_SHAKE_POWER;
        velocity.y += powerY * EFFECT_SCREEN_SHAKE_POWER;
    }

    void FixedUpdate ()
    {
        velocity.x -= velocity.x * drag;
        velocity.y -= velocity.y * drag;

        velocity.x -= ( position.x ) * elasticity;
        velocity.y -= ( position.y ) * elasticity;

        position.x += ( velocity.x );
        position.y += ( velocity.y );

        //Debug.Log ( "Velocity " + velocity );
        //Debug.Log ("Target position: " + position );
        target.transform.position = new Vector3 ( position.x, position.y, -5f );
    }
}
