using UnityEngine;
using System.Collections;

public class Test : MonoBehaviour {

    // Use this for initialization
    void Start() {
        SoundManager.Instance.Play( SoundManager.Sounds.SENegative );
    }
}
