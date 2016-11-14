using UnityEngine;
using System.Collections;

public class ControlAttackZombie : MonoBehaviour {

	// Use this for initialization
	static string ATTACKING = "attacking";
	bool attacking = false;

	ControlWarrior ctrlWarrior;
	public Transform zombie;
	Animator animZombie;

	public Vector2 currentPosition;
	void Start () {
		animZombie = zombie.gameObject.GetComponent<Animator> ();
	}

	// Update is called once per frame
	void FixedUpdate () {
		if (animZombie.GetCurrentAnimatorStateInfo (0).IsName (ATTACKING)) {
			if (!attacking) {
				attacking = true;
				upCollider ();

				Invoke ("downCollider", 0.5f);
			}
		} else {
			attacking = false;
		}


	}
	void upCollider (){
		currentPosition = transform.position;
		currentPosition.y += 4.1f;
		transform.position = currentPosition;
	}
	void downCollider (){
		currentPosition = transform.position;
		currentPosition.y -= 4.1f;
		transform.position = currentPosition;
	}

	void OnTriggerEnter2D (Collider2D other)
	{
		string tag = other.gameObject.tag;
		if (tag == "hero" && animZombie.GetCurrentAnimatorStateInfo (0).IsName (ATTACKING)) {
			ctrlWarrior = other.gameObject.gameObject.GetComponent<ControlWarrior> ();
			ctrlWarrior.sustractEnergy ();
		}
	}
}
