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
		if (other.tag == "NPC") {
			if(!DialogueCanvas.activeSelf){
				DialogueCanvas.SetActive (true);
			}
		}
	}
	void OnTriggerExit(Collider other){
		if (other.tag == "NPC") {
			if(DialogueCanvas.activeSelf){
				DialogueCanvas.SetActive (false);
			}
		}
	}
}
