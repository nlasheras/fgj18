using UnityEngine;

public class Spike : MonoBehaviour
{
    void OnTriggerEnter2D ( Collider2D other )
    {
        Debug.Log ( "HITSPIKE!" );
        if ( other.CompareTag ( "Player" ) )
        {
            FindObjectOfType<GameManager>().PlayerDied();
        }
    }
}
