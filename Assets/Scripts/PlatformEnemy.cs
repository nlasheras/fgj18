using UnityEngine;

public class PlatformEnemy : PlatformController
{
    private void OnTriggerEnter2D ( Collider2D other )
    {
        if ( !other.gameObject.CompareTag ( "Player" ) )
            return;

        Debug.Log ( "Player Got hit by the enemy!!" );

        GameManager.Instance.PlayerDied ();
    }

}
