using UnityEngine;
using System.Collections;

public class Fisix : MonoBehaviour {

	public int cnt = 0;
	public int step = 3;

	public Uchiwa uci;


	// Use this for initialization
	void Start () {
	
	}

	// Update is called once per frame
	void Update () {

		if (Input.GetKey (KeyCode.RightArrow)) {
			if (uci.isIkActiveLeft == false && uci.isIkActiveReight == false) {
					uci.isIkActiveReight = true;
			}
		} else if (Input.GetKey (KeyCode.LeftArrow)) {
			if (uci.isIkActiveLeft == false && uci.isIkActiveReight == false) {
				uci.isIkActiveLeft = true;
			}
		}
		if (cnt++ % step == 0) {
			if (Input.GetKey (KeyCode.UpArrow)) {
//					rigidbody.AddForce (Vector3.up * 50, ForceMode.Acceleration);
			}
			else if (uci.isIkActiveReight){
				transform.Rotate (new Vector3 (0, -10.0f, 0) * Time.deltaTime);
				rigidbody.AddForce (Vector3.up * 25, ForceMode.Acceleration);

			}
			else if (uci.isIkActiveLeft) {
				transform.Rotate (new Vector3 (0, 10.0f, 0) * Time.deltaTime);
				rigidbody.AddForce (Vector3.up * 25, ForceMode.Acceleration);
			}
		} else {
			rigidbody.AddForce (Vector3.up * Random.Range (0.0f, 12.0f), ForceMode.Acceleration);
			rigidbody.AddForce (Vector3.left * Random.Range (-50.0f, 50.0f), ForceMode.Acceleration);
			rigidbody.AddForce (transform.forward * Random.Range (10.0f, 15.0f), ForceMode.Acceleration);
		}
	}
}
