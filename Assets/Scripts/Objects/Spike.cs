using UnityEngine;

public class Spike : MonoBehaviour
{
    void OnTriggerEnter2D ( Collider2D other )
    {
        if ( other.CompareTag ( "Player" ) )
        {
            FindObjectOfType<GameManager>().PlayerDied();
        }
    }
}
