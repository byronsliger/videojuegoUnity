using UnityEngine;
using System.Collections;

public class ControlRuby : MonoBehaviour
{

	Animator anim;
	bool disappear = false;
	public AudioClip rubyCatch;
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

	public void disappearRuby ()
	{
		disappear = true;
		anim.SetTrigger ("disappear");
		aSource.PlayOneShot (rubyCatch);
	}

	public bool isDisappear() {
		return disappear;
	}
}
