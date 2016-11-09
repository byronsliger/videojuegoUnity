using UnityEngine;
using System.Collections;

public class ControlRuby : MonoBehaviour
{

	Animator anim;
	bool disappear = false;

	// Use this for initialization
	void Start ()
	{
		anim = GetComponent<Animator> ();
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}

	public void disappearRuby ()
	{
		disappear = true;
		anim.SetTrigger ("disappear");
	}

	public bool isDisappear() {
		return disappear;
	}
}
