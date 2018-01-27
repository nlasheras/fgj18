using UnityEngine;
using System.Collections;

public class DisplayRestartText : MonoBehaviour
{
    private Texture2D t2D;
    private float x, y;
    // Use this for initialization
    void Start ()
    {
        t2D = Resources.Load<Texture2D> ( "restart-text" );
        x = ( Screen.width - t2D.width ) / 2;
        y = Screen.height - 50;
    }

    void OnGUI ()
    {
        if ( Time.time % 2 > 1 )
            GUI.DrawTexture ( new Rect ( x, y, t2D.width, t2D.height ), t2D );
    }
}
