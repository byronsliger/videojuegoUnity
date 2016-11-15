using UnityEngine;
using System.Collections;

public class ControlSign : MonoBehaviour {
	ControlScene ctrlScene;

	public string message = "";
	bool isRead = false;

	// Use this for initialization
	void Start () {
		ctrlScene = GameObject.Find ("scene").GetComponent<ControlScene> ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter2D (Collider2D other){
		string tag = other.gameObject.tag;
		if (tag == "hero" && !isRead) {
			isRead = true;
			ctrlScene.showInstructions (message);	
		}
	}

}
