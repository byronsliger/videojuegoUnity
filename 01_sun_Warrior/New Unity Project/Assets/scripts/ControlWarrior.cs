using UnityEngine;
using System.Collections;

public class ControlWarrior : MonoBehaviour {
	static float walkVel = 3f;
	static float runVel = 4f;
	static string WALKING = "walking";
	static string RUNNING = "running";
	static string JUMPING = "jumping";
	static string IDLING = "idling";

	Rigidbody2D rbd;
	Animator animator;
	float currentVel = 3f;
	bool haciaDerecha = true;


	// Use this for initialization
	void Start () {
		rbd = GetComponent<Rigidbody2D> ();
		animator = GetComponent<Animator> ();
	}

	// Update is called once per frame
	void Update () {

		if(Input.GetKey (KeyCode.UpArrow)) {
			jump ();
		}

	}
	
	// Update is called once per frame
	void FixedUpdate () {
		float v = Input.GetAxis ("Horizontal") * currentVel;

		float speed = v < 0 ? v * -1 : v;

		animator.SetFloat ("speed", speed);
		if (!animator.GetCurrentAnimatorStateInfo (0).IsName (JUMPING) && Input.GetKey (KeyCode.UpArrow)) {
			
		}
		Vector2 vel = new Vector2 (0, rbd.velocity.y);
		if (animator.GetCurrentAnimatorStateInfo (0).IsName (WALKING)) {
			vel.x = v * walkVel;
			currentVel = walkVel;
		} else if (animator.GetCurrentAnimatorStateInfo (0).IsName (RUNNING)) { 
			vel.x = v * runVel;
			currentVel = runVel;
		}
		rbd.velocity = vel;	

		if (haciaDerecha && v < 0) {
			haciaDerecha = false;
			flip ();
		}else if(!haciaDerecha && v > 0){
			haciaDerecha = true;
			flip ();
		}
	}

	void jump () {
		//animator.SetTrigger ("jump");
		rbd.AddForce(new Vector2(0, 10), ForceMode2D.Impulse);
	}

	void flip(){
		var scale = transform.localScale;
		scale.x *= -1;
		transform.localScale = scale;
	}
}
