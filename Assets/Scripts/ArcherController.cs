using UnityEngine;
using System.Collections;


/// <summary>
/// Archer controller is attach to GameObject Archer01
/// </summary>
public class ArcherController : MonoBehaviour {
	private Animator ArcherAnimator;

	// Use this for initialization
	void Start () {	
	    ArcherAnimator = gameObject.GetComponent<Animator> ();
	}
	
	// Update is called once per frame
	void Update () {
		NPCNavigationAnimation();
	}
	private void NPCGetInput(){
		if (Input.GetKeyDown (KeyCode.J)) {
			ArcherAnimator.SetTrigger ("Attack1");
		}
		if (Input.GetKeyDown (KeyCode.Space)) {
			ArcherAnimator.SetTrigger ("Jump");
		}

		if (Input.GetKeyDown (KeyCode.W)) {
			ArcherAnimator.SetBool ("Run",true);
		}
		if (Input.GetKeyUp (KeyCode.W)) {
			ArcherAnimator.SetBool("Run",false);
		}
	}


	private void NPCNavigationAnimation(){
		if (Mathf.Abs(GetComponent<Rigidbody> ().velocity.y) > 0.0f) {
			ArcherAnimator.SetBool ("Run", true);
		} else {
			ArcherAnimator.SetBool ("Run", false);
		}

	}
}
