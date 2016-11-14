using UnityEngine;
using System.Collections;
using UnityEngine.Analytics;
using System.Collections.Generic;

public class ControlScene : MonoBehaviour {
	public float version = 0.1f;

	// Use this for initialization
	void Start () {
		GameStart ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void GameStart() {
		Analytics.CustomEvent ("GameStart", new Dictionary<string, object>{});
	}

	public void GameOver() {
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
}
