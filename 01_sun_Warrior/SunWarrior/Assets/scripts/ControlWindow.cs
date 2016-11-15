using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ControlWindow : MonoBehaviour {

	public Button accept;
	public Text tMessage;
	ControlScene ctrlScene;

	void Start () {
		accept.onClick.AddListener(TaskOnClick);
		ctrlScene = GameObject.Find ("scene").GetComponent<ControlScene> ();
	}

	public void setMesssage(string message){
		tMessage.text = message;
	}

	void TaskOnClick(){
		ctrlScene.hiedeInstructions ();
	}
}
