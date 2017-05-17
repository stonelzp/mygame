using UnityEngine;
using System.Collections;

public class PlayerControllerTest : MonoBehaviour {
	
	private Animation AnimationPlay;
	private Rigidbody PlayerRigidbody;

	private uint StatusPlayer=0;//0000 0000 0000 0000 0000 0000 0000 0000




	// Use this for initialization
	void Start () {
		AnimationPlay = gameObject.GetComponent<Animation> ();
		PlayerRigidbody = gameObject.GetComponent<Rigidbody> ();
	}
	
	// Update is called once per frame
	void Update () {
		statuscontroller ();
		if (AnimationPlay.IsPlaying ("Battle_Idle")) {
			Debug.Log ("true");
		} else {
			Debug.Log ("false");
		}
	}

	void statuscontroller(){
		if (Input.GetKey (KeyCode.I)) {
			AnimationPlay ["Run_Attack"].wrapMode = WrapMode.Once;
			AnimationPlay.CrossFade ("Run_Attack",0.3f);
			AnimationPlay.CrossFadeQueued ("Battle_Idle",0.3f);


		}
	}
}
