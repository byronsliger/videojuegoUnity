using UnityEngine;
using System.Collections;

public class ControlWarrior : MonoBehaviour {
	Rigidbody2D rbd;
	Animator animator;
	float malVel = 5f;
	bool haciaDerecha = true;
	public float velocity;

	// Use this for initialization
	void Start () {
		rbd = GetComponent<Rigidbody2D> ();
		animator = GetComponent<Animator> ();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		float v = Input.GetAxis ("Horizontal") * malVel;
		Vector2 vel = new Vector2 (0, rbd.velocity.y);
		vel.x = v;
		rbd.velocity = vel;
		velocity = vel.x;

		animator.SetFloat ("speed", v);

		if (haciaDerecha && v < 0) {
			haciaDerecha = false;
			flip ();
		}else if(!haciaDerecha && v > 0){
			haciaDerecha = true;
			flip ();
		}
	}

	void flip(){
		var scale = transform.localScale;
		scale.x *= -1;
		transform.localScale = scale;
	}
}
