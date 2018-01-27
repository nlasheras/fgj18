using UnityEngine;

public class ScreenShaker : MonoBehaviour
{
    private Vector2 _velocity;
    private Vector2 _position;
    private Camera _target;
    private float _drag = .1f;
    private float _elasticity = .1f;
    private float EFFECT_SCREEN_SHAKE_POWER = 0.009f;
    
    // Use this for initialization
    void Start ()
    {
        _target = Camera.main;
    }

    public void Shake ( float powerX, float powerY )
    {
        _velocity.x += powerX * EFFECT_SCREEN_SHAKE_POWER;
        _velocity.y += powerY * EFFECT_SCREEN_SHAKE_POWER;
    }

    // Update is called once per frame
    void FixedUpdate ()
    {
        _velocity.x -= _velocity.x * _drag;
        _velocity.y -= _velocity.y * _drag;

        _velocity.x -= ( _position.x ) * _elasticity;
        _velocity.y -= ( _position.y ) * _elasticity;

        _position.x += ( _velocity.x );
        _position.y += ( _velocity.y );

        _target.transform.position = new Vector3 ( _position.x, _position.y, -5f );
    }
}
