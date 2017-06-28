using UnityEngine;
using System.Collections;

public class DialogueTrigger : MonoBehaviour {
	public GameObject Player;
	public GameObject NPCPlayer;
	public GameObject DialogueCanvas;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter(Collider other){
		//if the collder is NPC,active DialogueCanvas
		if (other.tag == "NPC") {
			if(!DialogueCanvas.activeSelf){
				DialogueCanvas.SetActive (true);
			}

			//NPC has searched Player,then cancel NPC's Nav Mesh Agent and script NPCSearchPlayer
			//tell NPCPlayer to stop the Navigation
			NPCPlayer.GetComponent<ArcherController>().NavigationEnd();

			//Player Stop to Controller
			if(Player.GetComponent<PlayerController>().enabled){
				Player.GetComponent<PlayerController> ().DialogueAnimationTalkPlay ();
				Player.GetComponent<PlayerController> ().enabled = false;
			}

			NPCPlayer.GetComponent<Animator> ().SetBool ("Run",false);
		}


	}

	//本来是想离开一定范围之后会结束交谈结果是撞到触发器的时候，有可能“接触不良”，导致离开碰撞体触发下面的函数，暂时封印
//	void OnTriggerExit(Collider other){
//		if (other.tag == "NPC") {
//			if(DialogueCanvas.activeSelf){
//				DialogueCanvas.SetActive (false);
//			}
//		}
//	}
}
