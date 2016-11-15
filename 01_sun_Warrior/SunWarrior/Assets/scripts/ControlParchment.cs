using UnityEngine;
using System.Collections;

public class ControlParchment : MonoBehaviour
{
	Animator anim;
	bool disappear = false;
	public AudioClip parchmentCatch;
	AudioSource aSource;

	// Use this for initialization
	void Start ()
	{
		anim = GetComponent<Animator> ();
		aSource = GetComponent<AudioSource> ();
	}
	
	// Update is called once per frame
	void Update ()
	{
		
	}

	public void disappearParchment ()
	{
		disappear = true;
		anim.SetTrigger ("disappear");
		aSource.PlayOneShot (parchmentCatch);
	}

	public bool isDisappear() {
		return disappear;
	}

	void OnTriggerEnter2D (Collider2D other){
		string tag = other.gameObject.tag;
		if (tag == "hero") {
			if (!disappear) {
				disappearParchment ();
				other.gameObject.GetComponent<ControlWarrior> ().addOneToParchmentObtained ();	
			}
		}	
	}
}
