using UnityEngine;
using System.Collections;

public class ControlChest : MonoBehaviour {
	ControlWarrior ctrlWarrior;
	ControlScene ctrlScene;

	public AudioClip winTheLevel;
	AudioSource aSource;
	// Use this for initialization
	void Start () {
		ctrlWarrior = GameObject.Find ("warrior").GetComponent<ControlWarrior> ();
		ctrlScene = GameObject.Find ("scene").GetComponent<ControlScene> ();
		aSource = GetComponent<AudioSource> ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter2D (Collider2D other){
		string tag = other.gameObject.tag;
		if (tag == "hero" && ctrlWarrior.parchmentObtained > 0) {
			ctrlWarrior.gameOver = true;
			aSource.PlayOneShot (winTheLevel);
			ctrlScene.LevelOver ();
		}
	}
}
