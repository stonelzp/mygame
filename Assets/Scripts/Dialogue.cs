using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Xml;
using System.IO;
public class Dialogue : MonoBehaviour {
	private string[] dialogue_strings;


	// Use this for initialization
	void Start () {
		dialogue_init();

	}
	
	// Update is called once per frame
	void Update () {
	
	}


	private void dialogue_init(){
		//dialogue_strings[0] = "";
		//load xml
		string filePath=Application.dataPath+"/Resource/Story/dialogue.xml";
		if (File.Exists (filePath)) {
			Debug.Log ("Story file is found!");
			XmlDocument xmldoc = new XmlDocument ();
			xmldoc.Load (filePath);
			XmlNodeList node = xmldoc.SelectSingleNode ("dialogue").ChildNodes;
			foreach (XmlElement nodelist in node) {
				//get scene-altar dialogue data
				if (nodelist.Name == "scene-altar") {
					int i = 0;
					dialogue_strings=new string[nodelist.ChildNodes.Count*2];
					foreach (XmlElement dialoguelist in nodelist) {
						if (dialoguelist.Attributes ["id"].Value != "0") {
							Debug.Log ("The story is confused!");
						}
						dialogue_strings [i] = dialoguelist.SelectSingleNode ("charactor").InnerText;
						dialogue_strings [i+1] = dialoguelist.SelectSingleNode ("value").InnerText;
						i += 2;
					}
					break;
				}
			}

		} else {
			Debug.Log ("Story file is missing!");
		}


		//show dialogue_string
		foreach (string i in dialogue_strings) {
			Debug.Log (i);
		}
	}
}
