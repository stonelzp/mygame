using UnityEngine;
using System.Collections;

public class NPCSearchPlayer : MonoBehaviour {
	public Transform TargetObject;
	// Use this for initialization
	void Start () {
		gameObject.GetComponent<ArcherController> ().NavigationStart();
	}
	
	// Update is called once per frame
	void Update () {
	}
}
