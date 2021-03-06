﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
/// <summary>
/// attach to GameObject TransportUI
/// </summary>
public class TransportController : MonoBehaviour {
	public GameObject LeaveButton;//the GameObject "TransportUI/leave"
	public GameObject StayButton;//the GameObject "TransportUI/stay"
	public GameObject TransportTriggerObject;//the Object "TransportObjects/Trigger"
	//false is stay,true is leave
	private bool SelectedButton;
	private Color newColor;
	// Use this for initialization
	void Start () {
		newColor = LeaveButton.GetComponent<Image> ().color;
		newColor.a = 0.6f;
		SelectedButton = true;
		LeaveButton.GetComponent<Image> ().color = newColor;
		newColor.a = 0.3f;
		StayButton.GetComponent<Image> ().color = newColor;
	}
	
	// Update is called once per frame
	void Update () {
        if (SelectedButton) {
			newColor.a = 0.6f;
			LeaveButton.GetComponent<Image> ().color = newColor;
			newColor.a = 0.3f;
			StayButton.GetComponent<Image> ().color = newColor;
		} else {
			newColor.a = 0.3f;
			LeaveButton.GetComponent<Image> ().color = newColor;
			newColor.a = 0.6f;
			StayButton.GetComponent<Image> ().color = newColor;
		}
		if(Input.GetKeyDown(KeyCode.UpArrow)){
			if(!SelectedButton){
				SelectedButton = true;
			}
		}
		if(Input.GetKeyDown(KeyCode.DownArrow)){
			if(SelectedButton){
				SelectedButton = false;
			}
		}

		if(Input.GetKeyDown(KeyCode.Return)){
			if (SelectedButton) {
				Debug.Log ("跳转到下一个场景。");
                NextSceneToLoad();
			} else {
				if(gameObject.activeSelf){
					gameObject.SetActive (false);
				}
				TransportTriggerObject.GetComponent<TransportTrigger> ().NoticeFromTransportUI ();
			}
		}
	}

    //switch to the next scene
    private void NextSceneToLoad()
    {
        switch (Application.loadedLevelName)
        {
            case "backyard":
                SceneManager.LoadScene("field01");
                break;
            case "field01":
                SceneManager.LoadScene("home");
                break;
            case "home":
                SceneManager.LoadScene("field02");
                break;
            case "field02":
                SceneManager.LoadScene("village");
                break;
            case "village":
                SceneManager.LoadScene("field03");
                break;
            case "field03":
                SceneManager.LoadScene("backyard");
                break;
            default:
                break;
        }
    }



}
