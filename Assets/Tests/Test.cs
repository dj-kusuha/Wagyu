using UnityEngine;
using System.Collections;

public class Test : MonoBehaviour {

    // Use this for initialization
    void Start() {
        var soundManager = SoundManager.Instance;
        Debug.Log( soundManager );
    }
}
