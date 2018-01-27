using UnityEngine;

public class Explode : MonoBehaviour
{
    public BodyPart bodyPart;
    public int totalParts = 5;

    void OnTriggerEnter2D ( Collider2D target )
    {
        if ( target.gameObject.tag == "Deadly" )
        {
            OnExplode ();
        }
    }

    void OnCollisionEnter2D ( Collision2D target )
    {
        if ( target.gameObject.tag == "Deadly" )
        {
            OnExplode ();
        }
    }

    public void OnExplode ()
    {
        Destroy ( gameObject );
        Transform t = transform;

        for ( int i = 0; i < totalParts; i++ )
        {
            t.TransformPoint ( 0, -100, 0 );
            BodyPart clone = Instantiate (bodyPart, t.position, Quaternion.identity) as BodyPart;
            clone.GetComponent<Rigidbody2D> ().AddForce ( Vector2.right * Random.Range ( -50, 50 ) );
            clone.GetComponent<Rigidbody2D> ().AddForce ( Vector2.up * Random.Range ( 100, 400 ) );
        }

        GameObject go = new GameObject("RestartText");
        go.AddComponent<DisplayRestartText> ();
    }
}
