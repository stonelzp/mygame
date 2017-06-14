using UnityEngine;
using System.Collections;

public class ArcherController : MonoBehaviour {
	private Animator ArcherAnimator;

	// Use this for initialization
	void Start () {	
	    ArcherAnimator = gameObject.GetComponent<Animator> ();
	}
	
	// Update is called once per frame
	void Update () {
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
}
