using UnityEngine;
using System.Collections;

public class ControlAttackWarrior : MonoBehaviour {

	// Use this for initialization
	static string ATTACKING = "attacking";
	bool attacking = false;

	ControlZombie ctrlZombie; 
	public Transform warrior;
	Animator animWarrior;

	public Vector2 currentPosition;
	void Start () {
		animWarrior = warrior.gameObject.GetComponent<Animator> ();
	}

	// Update is called once per frame
	void FixedUpdate () {
		if (animWarrior.GetCurrentAnimatorStateInfo (0).IsName (ATTACKING)) {
			if (!attacking) {
				attacking = true;
				upCollider ();

				Invoke ("downCollider", 0.3f);
			}
		} else {
			attacking = false;
		}


	}
	void upCollider (){
		currentPosition = transform.position;
		currentPosition.y += 5;
		transform.position = currentPosition;
	}
	void downCollider (){
		currentPosition = transform.position;
		currentPosition.y -= 5;
		transform.position = currentPosition;
	}

	void OnTriggerEnter2D (Collider2D other)
	{
		string tag = other.gameObject.tag;
		if (tag == "zombie" && animWarrior.GetCurrentAnimatorStateInfo (0).IsName (ATTACKING)) {
			ctrlZombie = other.gameObject.gameObject.GetComponent<ControlZombie> ();
			ctrlZombie.sustractEnergy ();
		}
	}
}
