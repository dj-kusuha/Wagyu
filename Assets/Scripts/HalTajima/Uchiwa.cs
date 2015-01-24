using UnityEngine;
using System.Collections;

public class Uchiwa : MonoBehaviour {
	
	public Animator anim;
	public bool isIkActiveReight = false;
	public bool isIkActiveLeft = false;
	public float mixWeight = 1.0f;

	public Transform RightUchiwa;
	public Transform LeftUchiwa;

	public AnimationCurve pata;

	private float tim = 0;


	// Use this for initialization
	void Start () {
	}
	void Awake ()
	{
		anim = GetComponent<Animator> ();
//		LeftHandOrg = LeftHand;
//		RightHandOrg = RightHand;

	}


	void Update(){
		if (isIkActiveReight) {
			if(tim > 1.0f){
				tim = 0.0f;
				isIkActiveReight = false;
			}
			float y = pata.Evaluate(tim);
			tim = tim + Time.deltaTime;
			Vector3 w = new Vector3(RightUchiwa.localPosition.x,y,RightUchiwa.localPosition.z);
			RightUchiwa.localPosition = w;
		}
		else if (isIkActiveLeft) {
			if(tim > 1.0f){
				tim = 0.0f;
				isIkActiveLeft = false;
			}
			float y = pata.Evaluate(tim);
			tim = tim + Time.deltaTime;
			Vector3 w = new Vector3(LeftUchiwa.localPosition.x,y,LeftUchiwa.localPosition.z);
			LeftUchiwa.localPosition = w;
		}

	}


	void OnAnimatorIK (int layerIndex)
	{
		if (isIkActiveReight) {
			anim.SetIKPositionWeight (AvatarIKGoal.RightHand, mixWeight);
			anim.SetIKRotationWeight (AvatarIKGoal.RightHand, 0);
			anim.SetIKPosition (AvatarIKGoal.RightHand, RightUchiwa.position);
		}
		if (isIkActiveLeft) {
			anim.SetIKPositionWeight (AvatarIKGoal.LeftHand, mixWeight);
			anim.SetIKRotationWeight (AvatarIKGoal.LeftHand, 0);
			anim.SetIKPosition (AvatarIKGoal.LeftHand, LeftUchiwa.position);
		}
	}
	
	// Update is called once per frame
}
