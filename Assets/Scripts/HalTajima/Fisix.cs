using UnityEngine;
using System.Collections;

public class Fisix : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}

	// Update is called once per frame
	void Update () {
		if (Input.GetKey (KeyCode.UpArrow)) {
			rigidbody.AddForce (Vector3.up * 50,	ForceMode.Acceleration);
		} else if (Input.GetKey (KeyCode.RightArrow)) {
			transform.Rotate(new Vector3(0, 10, 0) * Time.deltaTime);
		} else if (Input.GetKey (KeyCode.LeftArrow)) {
			transform.Rotate(new Vector3(0, -10, 0) * Time.deltaTime);
		}
		rigidbody.AddForce (Vector3.up * Random.Range(0.0f, 12.0f),ForceMode.Acceleration);
//		rigidbody.AddForce (Vector3.left * Random.Range(-50.0f, 50.0f),ForceMode.Acceleration);
		rigidbody.AddForce (transform.forward * Random.Range(5.0f, 25.0f),ForceMode.Acceleration);
	}
}
