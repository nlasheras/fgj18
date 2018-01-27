using UnityEngine;

public class SingletonBehaviour<T> : MonoBehaviour where T : MonoBehaviour
{
    public static T Instance { get; private set; }

    public void RegisterSingleton ()
    {
        if ( Instance != null )
        {
            Debug.LogWarning ( "Trying to register another SingletonBehaviour of " + typeof ( T ) );
            Destroy ( this.gameObject );
        }
        else
        {
            Instance = this as T;
            DontDestroyOnLoad ( this.gameObject );
        }
    }

    public void UnregisterSingleton ()
    {
        if ( Instance == this )
        {
            Instance = null;
        }
        else if ( Instance == null )
        {
#if UNITY_EDITOR
            if (UnityEditor.EditorApplication.isPlaying)
#endif
            Debug.LogError ( "UnregisterSingleton called for SingletonBehaviour of " + typeof ( T ) );
        }
    }
}