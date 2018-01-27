using UnityEngine;
using System.Collections;

public class PlatformEnemy : PlatformController
{
    private Transform target;
 
    // Use this for initialization
    public override void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
        base.Start(); 
    }

    void Update()
    {
        float sqrRemainingDistance = (transform.position - target.position).sqrMagnitude;

        if (sqrRemainingDistance <= 3.0f)
        {
            Debug.Log("Player Got hit by the enemy!!");
            //Destroy(target.gameObject);
        }
    }

}
