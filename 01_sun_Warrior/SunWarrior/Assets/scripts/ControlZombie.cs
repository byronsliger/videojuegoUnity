using UnityEngine;
using System.Collections;

public class ControlZombie : MonoBehaviour {
	public float vel = -1;
	Rigidbody2D rbd;
	Animator animator;
	static string WALKING = "walking";
	static string IDLING = "idling";
	// Use this for initialization
	void Start () {
		rbd = GetComponent<Rigidbody2D> ();
		animator = GetComponent<Animator> ();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		Vector2 vector = new Vector2 (vel, 0);

		if (animator.GetCurrentAnimatorStateInfo (0).IsName (WALKING) ) {
			rbd.velocity = vector;	
		}

		if (animator.GetCurrentAnimatorStateInfo (0).IsName (WALKING) && Random.value < 1f / (60f * 14f)) {
			animator.SetTrigger ("idle");
		} else if (animator.GetCurrentAnimatorStateInfo (0).IsName (IDLING) && Random.value < 1f / (60f * 3f)) {
			animator.SetTrigger ("walk");
		}


	}

	void OnTriggerEnter2D(Collider2D other) {
		flip ();
	}

	void flip(){
		vel *= -1;
		var scale = transform.localScale;
		scale.x *= -1;
		transform.localScale = scale;
	}
}
