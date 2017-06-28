using UnityEngine;
using System.Collections;
using UnityEngine.AI;


/// <summary>
/// Archer controller is attach to GameObject Archer01
/// </summary>
public class ArcherController : MonoBehaviour {
	public Transform targetPlayerPosition;
	//the destination of NPCPlayer
	private Transform targetDestination;
	private Animator ArcherAnimator;
	private bool isSearching=false;
	public Transform StartPositon;

	// Use this for initialization
	void Start () {	
	    ArcherAnimator = gameObject.GetComponent<Animator> ();
		ArcherAnimator.Play("Idle");
	}
	
	// Update is called once per frame
	void Update () {
		//NPCNavigationAnimation();
		if (isSearching) {
			NPCNavigation ();
		} else {
			NPCDialogueStatus ();
		}
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


//	private void NPCNavigationAnimation(){
//		bool velocity_x = (Mathf.Abs (GetComponent<Rigidbody> ().velocity.x) > 0.0f);
//		bool velocity_y = (Mathf.Abs (GetComponent<Rigidbody> ().velocity.y) > 0.0f);
//		bool velocity_z = (Mathf.Abs (GetComponent<Rigidbody> ().velocity.z) > 0.0f);
//		Debug.Log (velocity_x);
//		Debug.Log (velocity_y);
//		Debug.Log (velocity_z);
//
//		if (velocity_y||velocity_x||velocity_z) {
//			ArcherAnimator.SetBool ("Run", true);
//		} else {
//			ArcherAnimator.SetBool ("Run", false);
//		}
//
//	}

	private void NPCPlayAnimation(string animation_name){
		switch (animation_name) {
		case "Idle":
			ArcherAnimator.Play ("Idle");
			break;
		case "Run":
			ArcherAnimator.SetBool ("Run", true);
			break;
		default:
			break;
		}
	}


	private void NPCNavigation(){
		if(targetDestination!=null){
			if (!gameObject.GetComponent<NavMeshAgent> ().enabled) {
				gameObject.GetComponent<NavMeshAgent> ().enabled = true;
			}
			gameObject.GetComponent<NavMeshAgent> ().destination = targetDestination.position;
		}
		ArcherAnimator.SetBool ("Run",true);
	}
	private void NPCDialogueStatus(){
		if (gameObject.GetComponent<NavMeshAgent> ().enabled) {
			gameObject.GetComponent<NavMeshAgent> ().enabled = false;
		}
		ArcherAnimator.SetBool ("Run",false);
	}

	public void NavigationStart(){
		isSearching = true;
		targetDestination = targetPlayerPosition;
	}

	public void NavigationEnd(){
		isSearching = false;
	}

	public void NPCReturn(){
		isSearching = true;
		targetDestination = StartPositon;
	}


}
