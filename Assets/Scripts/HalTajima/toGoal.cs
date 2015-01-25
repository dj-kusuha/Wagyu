using UnityEngine;
using System.Collections;

public class toGoal : MonoBehaviour {

	public GameObject GoalObj;
	public float tim;

	public AnimationCurve x;
	public AnimationCurve y;
	public AnimationCurve z;

	private GameObject UnityChan;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void LateUpdate () {
		if (UnityChan != null && tim < 2.0f) {
			float xx;
			float yy;
			float zz;
			xx = x.Evaluate(tim);
			yy = y.Evaluate(tim);
			zz = z.Evaluate(tim);
			Vector3 a = new Vector3(xx,yy,zz);
			UnityChan.transform.position = new Vector3(xx,yy,zz);
			tim += Time.deltaTime;
		}


	}
	void OnCollisionEnter(Collision collision) {
		if(collision.gameObject.CompareTag("Player")){
			UnityChan = collision.gameObject;
			UnityChan.rigidbody.isKinematic = true;
			this.rigidbody.isKinematic = false;
			tim=0;
			x = new AnimationCurve();
			y = new AnimationCurve();
			z = new AnimationCurve();
			Keyframe keyX = new Keyframe();
			Keyframe keyY= new Keyframe();
			Keyframe keyZ= new Keyframe();
			keyX.time = 0.0f;
			keyY.time = 0.0f;
			keyZ.time = 0.0f;
			keyX.value = UnityChan.transform.position.x;
			keyY.value = UnityChan.transform.position.y;
			keyZ.value = UnityChan.transform.position.z;
			x.AddKey(keyX);
			y.AddKey(keyY);
			z.AddKey(keyZ);

			keyX = new Keyframe();
			keyY= new Keyframe();
			keyZ= new Keyframe();


			keyY.time = 1.0f;
			keyY.value = 4000.0f;
			y.AddKey(keyY);

			keyX = new Keyframe();
			keyY= new Keyframe();
			keyZ= new Keyframe();

			keyX.time = 2.0f;
			keyY.time = 2.0f;
			keyZ.time = 2.0f;
			keyX.value = GoalObj.transform.position.x;
			keyY.value = GoalObj.transform.position.y;
			keyZ.value = GoalObj.transform.position.z;
			x.AddKey(keyX);
			y.AddKey(keyY);
			z.AddKey(keyZ);
		}
	}
}
