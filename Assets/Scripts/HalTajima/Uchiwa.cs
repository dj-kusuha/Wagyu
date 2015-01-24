using UnityEngine;
using System.Collections;

public class Uchiwa : MonoBehaviour {
	
	public int cnt = 0;
	public int step = 3;
	public Animator anim;
	public bool isIkActive = false;
	public float mixWeight = 1.0f;

	public Transform LeftHand;
	public Transform RightHand;

	public Transform RightUchiwa;
	public Transform LeftUchiwa;
	private Transform LeftHandOrg;
	private Transform RightHandOrg;


	// Use this for initialization
	void Start () {
	}
	void Awake ()
	{
		anim = GetComponent<Animator> ();
//		LeftHandOrg = LeftHand;
//		RightHandOrg = RightHand;

	}



	void OnAnimatorIK (int layerIndex)
	{
//		Debug.Log("OnAnimatorIK");
		if (isIkActive) {
			anim.SetIKPositionWeight (AvatarIKGoal.RightHand, mixWeight);
			anim.SetIKRotationWeight (AvatarIKGoal.RightHand, 0);
			anim.SetIKPosition (AvatarIKGoal.RightHand, RightUchiwa.position);
//			anim.SetIKRotation (AvatarIKGoal.RightHand, RightHandOrg.rotation);
			anim.SetIKPositionWeight (AvatarIKGoal.LeftHand, mixWeight);
			anim.SetIKRotationWeight (AvatarIKGoal.LeftHand, 0);
			anim.SetIKPosition (AvatarIKGoal.LeftHand, LeftUchiwa.position);
//			anim.SetIKRotation (AvatarIKGoal.LeftHand, LeftHand.rotation);
			Debug.Log("OnAnimatorIK");
		}
	}
	
	// Update is called once per frame
}
