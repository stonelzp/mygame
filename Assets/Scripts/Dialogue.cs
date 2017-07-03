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
	public GameObject CharactorPicture;
	public Sprite spritePlayer;
	public Sprite spriteArcher;
	public Sprite spriteMage01;
	public Sprite spriteMage02;

	private string[] dialogue_strings;
	private Text _textComponent;
	private Text _textCharactor;

	private float SecondsBetweenCharacters = 0.1f;
	private int DialogueLength = 0;
	private int CurrentDialogueIndex = 0;

	private bool DialogueIsDisplaying=false;
    private bool DialogueSkip = false;

    private string DialogueName;

	// Use this for initialization
	void Start () {
		_textComponent = gameObject.GetComponent<Text> ();
		_textCharactor = dialogueCharactor.GetComponent<Text>();
        DialogueName = GlobalController.Instance.getDialogueSceneName();
		dialogue_init();


	}
	
	// Update is called once per frame
	void Update () {
		if (!DialogueIsDisplaying) {
			if(!dialogueContinue.activeSelf){
				dialogueContinue.SetActive (true);
			}
			if (Input.GetKeyDown (KeyCode.Return)) {
                showDialogueLogic();
			}
            //skip the dialogue
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                SecondsBetweenCharacters = Time.deltaTime;
                DialogueSkip = true;
            }
            if (DialogueSkip)
            {
                showDialogueLogic();
            }

		} else {
			if(dialogueContinue.activeSelf){
				dialogueContinue.SetActive (false);
			}
		}
	}
    //controll the Dialogue playing logic,how to paly and at the end of the paly how to deal with GameObjects.
    private void showDialogueLogic()
    {
        if (CurrentDialogueIndex < DialogueLength)
        {
            StartCoroutine(display_dialogue(dialogue_strings[CurrentDialogueIndex + 1], dialogue_strings[CurrentDialogueIndex]));
            CurrentDialogueIndex += 2;
        }
        else
        {
            /*这里有一个当Player对话完毕之后虽然结束了对话但是实际上还可以继续触发对话的问题，
            只是CurrentDialogueIndex >=DialogueLength的原因，
            对话的UI出现的同时因为这个条件再次被set false，虽然暂时没有什么影响。*/
            Debug.Log("The Story is end.");
            if (DialogueCanvas.activeSelf)
            {
                DialogueCanvas.SetActive(false);
            }
            if (!Player.GetComponent<PlayerController>().enabled)
            {
                Player.GetComponent<PlayerController>().enabled = true;
            }
            //tell the player the dialogue has ended.
            Player.GetComponent<PlayerController>().DialogueAnimationTalkPlayEnd();
            //tell the NPC the Dialogue has ended.
            if (GlobalController.Instance.getDialogueSceneName() == "scene-altar")
            {
                NPCPlayer.GetComponent<ArcherController>().NPCReturn();
            }


        }
    }

	private IEnumerator display_dialogue(string stringToDisplay,string stringCharactor){
//		Debug.Log (stringToDisplay);
//		Debug.Log (stringCharactor);
		int stringLength = stringToDisplay.Length;
		print (stringLength);
		int currentStringIndex = 0;
		DialogueIsDisplaying = true;
		_textComponent.text = "";
		_textCharactor.text = stringCharactor;
		if (stringCharactor == "星莲") {
			CharactorPicture.GetComponent<Image> ().sprite = spritePlayer;
		} else if (stringCharactor == "星白") {
			CharactorPicture.GetComponent<Image> ().sprite = spriteArcher;
		} else if (stringCharactor == "星闲") {
			CharactorPicture.GetComponent<Image> ().sprite = spriteMage01;
		} else if (stringCharactor == "星月") {
			CharactorPicture.GetComponent<Image> ().sprite = spriteMage02;
		}
		while (currentStringIndex < stringLength) {
			//text is overflow
			if (_textComponent.text.Length >= 42) {
				_textComponent.text = "";
			}
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
        //init the SecondsBetweenCharacters Attribute
        SecondsBetweenCharacters = 0.1f;
        //load xml
        string filePath=Application.dataPath+"/Resource/Story/dialogue.xml";
		if (File.Exists (filePath)) {
			Debug.Log ("Story file is found!");
			XmlDocument xmldoc = new XmlDocument ();
			xmldoc.Load (filePath);
			XmlNodeList node = xmldoc.SelectSingleNode ("dialogue").ChildNodes;
			foreach (XmlElement nodelist in node) {
                //get scene-altar dialogue data
                Debug.Log(DialogueName);
				if (nodelist.Name == DialogueName) {
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
		foreach (string i in dialogue_strings) {
			Debug.Log (i);
		}
	}
}
