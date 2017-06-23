using UnityEngine;
using System.Collections;

public class NPCSearchPlayer : MonoBehaviour {
	public Transform TargetObject;
	// Use this for initialization
	void Start () {
		if(TargetObject!=null){
			gameObject.GetComponent<UnityEngine.AI.NavMeshAgent> ().destination = TargetObject.position;
		}
	}
	
	// Update is called once per frame
	void Update () {
		if(TargetObject!=null){
			gameObject.GetComponent<UnityEngine.AI.NavMeshAgent> ().destination = TargetObject.position;
		}
	}
}
