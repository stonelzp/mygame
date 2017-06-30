using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Xml;
using System.IO;

[RequireComponent(typeof(Text))]
public class Dialogue : MonoBehaviour {
	public GameObject dialogueCharactor;
	public GameObject dialogueContinue;
	public GameObject DialogueCanvas;
	public GameObject Player;
	public GameObject NPCPlayer;

	private string[] dialogue_strings;
	private Text _textComponent;
	private Text _textCharactor;

	private float SecondsBetweenCharacters = 0.1f;
	private int DialogueLength = 0;
	private int CurrentDialogueIndex = 0;

	private bool DialogueIsDisplaying=false;


	// Use this for initialization
	void Start () {
		_textComponent = gameObject.GetComponent<Text> ();
		_textCharactor = dialogueCharactor.GetComponent<Text>();
		dialogue_init();


	}
	
	// Update is called once per frame
	void Update () {
		if (!DialogueIsDisplaying) {
			if(!dialogueContinue.activeSelf){
				dialogueContinue.SetActive (true);
			}
			if (Input.GetKeyDown (KeyCode.Return)) {
				if (CurrentDialogueIndex < DialogueLength) {
					StartCoroutine (display_dialogue (dialogue_strings [CurrentDialogueIndex + 1], dialogue_strings [CurrentDialogueIndex]));
					CurrentDialogueIndex += 2;
				} else {
					Debug.Log ("The Story is end.");
					if(DialogueCanvas.activeSelf){
						DialogueCanvas.SetActive (false);	
					}
					if (!Player.GetComponent<PlayerController> ().enabled) {
						Player.GetComponent<PlayerController> ().enabled = true;
					}
					//tell the player the dialogue has ended.
					Player.GetComponent<PlayerController> ().DialogueAnimationTalkPlayEnd ();
					//tell the NPC the Dialogue has ended.
					NPCPlayer.GetComponent<ArcherController>().NPCReturn();


				}
			}
		} else {
			if(dialogueContinue.activeSelf){
				dialogueContinue.SetActive (false);
			}
		}
	}

	private IEnumerator display_dialogue(string stringToDisplay,string stringCharactor){
//		Debug.Log (stringToDisplay);
//		Debug.Log (stringCharactor);
		int stringLength = stringToDisplay.Length;
		int currentStringIndex = 0;
		DialogueIsDisplaying = true;
		_textComponent.text = "";
		_textCharactor.text = stringCharactor;
		while (currentStringIndex < stringLength) {
			_textComponent.text += stringToDisplay [currentStringIndex];
			currentStringIndex++;
			if (currentStringIndex < stringLength) {
				yield return new WaitForSeconds (SecondsBetweenCharacters);
			} else {
				break;
			}

		}
		DialogueIsDisplaying = false;
	}

	private void dialogue_init(){
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
					int i = 0;//the index present to dialogue_strings
					int j = 0;//the index to check if the xml/body/id is correct
					DialogueLength=nodelist.ChildNodes.Count*2;
					dialogue_strings=new string[DialogueLength];
					foreach (XmlElement dialoguelist in nodelist) {
						//check if the story is normal played.
						if (dialoguelist.Attributes ["id"].Value != j.ToString()) {
							Debug.Log ("The story is confused!");
						}
						dialogue_strings [i] = dialoguelist.SelectSingleNode ("charactor").InnerText;
						dialogue_strings [i+1] = dialoguelist.SelectSingleNode ("value").InnerText;
						i += 2;
						j++;
					}
					break;
				}
			}

		} else {
			Debug.Log ("Story file is missing!");
		}

		//show dialogue_string
//		foreach (string i in dialogue_strings) {
//			Debug.Log (i);
//		}
	}
}
