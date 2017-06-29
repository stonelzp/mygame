using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransportTrigger : MonoBehaviour {
	public GameObject NPCPlayer;
	public GameObject Player;
	public GameObject Particle04;
	public GameObject Particle05;
	public GameObject TransportUI;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter(Collider other){
		if(other.tag=="NPC"){
			NPCPlayer.SetActive (false);
			if (Particle05.activeSelf) {
				Particle05.SetActive (false);
			}
			Particle05.SetActive (true);
		}
		if(other.tag=="Player"){
			if (!TransportUI.activeSelf) {
				TransportUI.SetActive (true);
				Player.GetComponent<PlayerController> ().DialogueAnimationTalkPlayEnd ();//set animation to Normal_Idle
				Player.GetComponent<PlayerController> ().enabled = false;
			}
				
				
		}
	}
}
