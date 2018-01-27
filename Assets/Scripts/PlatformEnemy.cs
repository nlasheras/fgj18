using UnityEngine;
using System.Collections;

public class PlatformEnemy : PlatformController
{
    private Transform target;
 
    // Use this for initialization
    public override void Start()
    {
        
        base.Start(); 
    }

    void Update()
    {
        if ( !target )
        {
            target = GameObject.FindGameObjectWithTag("Player").transform;
            return;
        }

        float sqrRemainingDistance = (transform.position - target.position).sqrMagnitude;

        if (sqrRemainingDistance <= 3.0f)
        {
            Debug.Log("Player Got hit by the enemy!!");
            Destroy(target.gameObject);
            
            FindObjectOfType<GameManager>().PlayerDied();
        }
    }

}
