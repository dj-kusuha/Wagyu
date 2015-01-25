using UnityEngine;
using System.Collections;

public class Fisix : MonoBehaviour {

	public int cnt = 0;
	public int step = 3;

	public float v_y;

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
//				rigidbody.AddForce (Vector3.up * Random.Range (11.0f, 12.0f), ForceMode.Acceleration);
//				rigidbody.AddForce (Vector3.up * Random.Range (0.275f, 0.3f), ForceMode.VelocityChange);

			}
			else if (uci.isIkActiveLeft) {
				transform.Rotate (new Vector3 (0, 10.0f, 0) * Time.deltaTime);
//				rigidbody.AddForce (Vector3.up  * Random.Range (11.0f, 12.0f), ForceMode.Acceleration);
//				rigidbody.AddForce (Vector3.up * Random.Range (0.275f, 0.3f), ForceMode.VelocityChange);
			}
		} else {

		}
		v_y = rigidbody.velocity.y;
//		if (transform.position.y < 4) {
			if(rigidbody.velocity.y < 0){
				rigidbody.AddForce (Vector3.up * Random.Range (0.275f, 0.3f), ForceMode.VelocityChange);
//				Debug.Log("Jump");
			}
//		}
		rigidbody.AddForce (Vector3.right * Random.Range (-50.0f, 50.0f), ForceMode.Acceleration);
		rigidbody.AddForce (transform.forward * Random.Range (20.5f, 25.5f), ForceMode.Acceleration);
		transform.localEulerAngles = new Vector3 (transform.localEulerAngles.x, transform.localEulerAngles.y, 0);

	}
}
