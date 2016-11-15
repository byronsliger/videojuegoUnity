using UnityEngine;
using System.Collections;
using UnityEngine.Analytics;
using System.Collections.Generic;
using UnityEngine.UI;

public class ControlScene : MonoBehaviour {
	public float version = 0.1f;
	public float time = 0.0f;

	public GameObject window;
	ControlWindow ctrlWindow;

	public AudioClip windowPause;
	AudioSource aSource;

	// Use this for initialization
	void Start () {
		GameStart ();
		ctrlWindow = window.GetComponent<ControlWindow> ();
		aSource = GetComponent<AudioSource> ();
		window.SetActive (false);

	}
	
	// Update is called once per frame
	void Update () {
		time = Time.timeScale;
	}

	public void pauseGame (){
		Time.timeScale = 0.0f;
	}

	public void resumeGame (){
		Time.timeScale = 1.0f;
	}

	public void GameStart() {
		Analytics.CustomEvent ("GameStart", new Dictionary<string, object>{});
	}

	public void LevelOver() {
		ControlWarrior crtlWarrior = GameObject.Find ("warrior").GetComponent<ControlWarrior> ();
		ControlZombie crtlZombie = GameObject.Find ("zombie").GetComponent<ControlZombie> ();
		float secsTime = Time.time;

		Analytics.CustomEvent ("GameOver", new Dictionary<string, object>{
			{"time", secsTime},
			{"energyWarrior", crtlWarrior.energy},
			{"rubiesCatched", crtlWarrior.numOfRubies},
			{"parchmentCatched", crtlWarrior.parchmentObtained},
			{"energyZombie", crtlZombie.currentEnergy},
			{"version", version}
		});
	}

	public void showInstructions (string message)
	{
		pauseGame ();
		ctrlWindow.setMesssage(message);
		window.SetActive(true);
		aSource.PlayOneShot (windowPause);

	}

	public void hiedeInstructions ()
	{
		resumeGame ();
		window.SetActive(false);
	}
}
