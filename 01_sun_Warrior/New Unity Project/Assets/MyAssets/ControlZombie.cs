using UnityEngine;
using System.Collections;

public class ControlZombie : MonoBehaviour {
	public float vel = -1;
	Rigidbody2D rbd;
	// Use this for initialization
	void Start () {
		rbd = GetComponent<Rigidbody2D> ();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		Vector2 vector = new Vector2 (vel, 0);
		rbd.velocity = vector;
	}
}
