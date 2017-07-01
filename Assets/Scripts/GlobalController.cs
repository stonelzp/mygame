using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalController : MonoBehaviour {
    public static GlobalController Instance;


    public string DialogueSceneName="";
    // Use this for initialization
    void Awake() {
        if (Instance==null) {
            DontDestroyOnLoad(gameObject);
            Instance = this;
        }else if (Instance!=this) {
            Destroy(gameObject);
        }
	}
	

    public void setDialogueSceneNameAttribute(string name) {
        switch (name) {
            case "scene-altar":
                GlobalController.Instance.DialogueSceneName = "scene-altar";
                break;
            case "scene-home":
                GlobalController.Instance.DialogueSceneName = "scene-home";
                break;
            default:
                Debug.Log("the dialogue has no scene name!");
                break;

        }
    }

    public string getDialogueSceneName()
    {
        return GlobalController.Instance.DialogueSceneName;
    }
}
