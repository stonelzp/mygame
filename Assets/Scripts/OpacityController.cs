//script is attched to GameObject Canvas/DialogueBg 
using UnityEngine;
using System.Collections;
using UnityEngine.UI;//add UI component reference

public class OpacityController : MonoBehaviour {

	//change the background opacity
	void Awake () {
		GetComponent<Image>().color=new Color(1f,1f,1f,0.6f);
	}
	
	// Update is called once per frame
	void Update () {
	}

}
