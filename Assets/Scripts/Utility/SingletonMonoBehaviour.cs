using UnityEngine;
using System.Collections;

public class SingletonMonoBehaviour<T> : MonoBehaviour where T : SingletonMonoBehaviour<T> {

    private static T _instance;
    public static T Instance {
        get {
            if( _instance != null ) {
                _instance = FindObjectOfType<T>();
                if( _instance == null ) {
                    Debug.LogWarning( typeof( T ) + " is nothing." );
                }
            }
            return _instance;
        }
    }

    virtual protected void Awake() {
        CheckInstance();
    }

    protected bool CheckInstance() {
        if( _instance == null ) {
            _instance = (T)this;
            return true;
        } else if( Instance == this ) {
            return true;
        }

        Destroy( this );
        return false;
    }
}