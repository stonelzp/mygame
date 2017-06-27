using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Monster attack area trigger. required Component Script:Follow Target  set offset (0,1.2,0)
/// </summary>
public class MonsterAttackAreaTrigger : MonoBehaviour {
	public MonsterController monster_controller;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter(Collider other){
		if (other.tag == "Player") {
			monster_controller.setBoolNearTarget (true);
		}
	}
}
