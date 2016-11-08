using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEditor;

public class ControlWarrior : MonoBehaviour
{
	static float walkVel = 3f;
	static float runVel = 3.5f;
	static string WALKING = "walking";
	static string RUNNING = "running";
	static string DYING = "Dying";

	const string pathTexturePotionOnTop = "Assets/Premios/PNG/64px/potion_6.png";
	const string pathTextureParchmentOnTop = "Assets/Premios/PNG/64px/scroll_5.png";
	const string pathTextureParchmentBlackOnTop = "Assets/Premios/PNG/64px/scroll_5_black.png";

	Rigidbody2D rbd;
	Animator animator;
	float currentVel = 2f;
	bool haciaDerecha = true;
	bool attacking = false;

	ControlParchment ctrlParchment;
	ControlRuby ctrlRuby;
	ControlZombie ctrlZombie;

	public int energy = 3;
	public int parchmentObtained = 0;
	public int numOfRubies = 0;
	public Text txtNumOfRubies;

	public bool jumping = false;
	public float yJumpForce = 300;
	Vector2 jumpForce;

	public bool onGround = true;
	public bool allowJump;
	public string numCollition;
	//public Canvas canvas;


	// Use this for initialization
	void Start ()
	{
		rbd = GetComponent<Rigidbody2D> ();
		animator = GetComponent<Animator> ();

		/*canvas = GameObject.Find ("Canvas").GetComponent<Canvas> ();

		Image[] images = canvas.GetComponentsInChildren<Image> ();
		foreach (Image image in images) {
			image.enabled = false;
		}*/

	}

	void OnGUI ()
	{
		drawEnergyHero ();
		drawParchmentFinded ();
	}

	// Update is called once per frame
	void Update ()
	{
		txtNumOfRubies.text = "x " + numOfRubies;
	}
		
	// Update is called once per frame
	void FixedUpdate ()
	{
		if (animator.GetCurrentAnimatorStateInfo (0).IsName (DYING)) {
			return;
		}

		move ();

		jump ();

		attack ();

	}

	void drawEnergyHero ()
	{
		Texture2D inputTexture = (Texture2D)UnityEditor.AssetDatabase.LoadAssetAtPath (pathTexturePotionOnTop, typeof(Texture2D));
		int x = 10;
		for (int i = 1; i <= energy; i++) {
			GUI.DrawTexture (new Rect (x, 10, 19, 32), inputTexture);
			x += 25;
		}
	}

	void drawParchmentFinded ()
	{
		Texture2D parchmentBlackOnTop = (Texture2D)UnityEditor.AssetDatabase.LoadAssetAtPath (pathTextureParchmentBlackOnTop, typeof(Texture2D));
		Texture2D parchmentOnTop = (Texture2D)UnityEditor.AssetDatabase.LoadAssetAtPath (pathTextureParchmentOnTop, typeof(Texture2D));
		int x = 505;
		for (int i = 1; i <= 3; i++) {
			if (parchmentObtained < i) {
				GUI.DrawTexture (new Rect (x, 10, 32, 31), parchmentBlackOnTop);
			} else { 
				GUI.DrawTexture (new Rect (x, 10, 32, 31), parchmentOnTop);
			}
			x -= 35;
		}
	}

	void jump ()
	{
		if (Input.GetAxis ("Jump") > 0.01f) {
			if (!jumping /*&& onGround */) {
				
				jumpForce.x = 0f;
				jumpForce.y = yJumpForce;
				animator.SetTrigger ("jump");
				rbd.AddForce (jumpForce);
				jumping = true;
				onGround = false;
			}
		} else { 
			jumping = false;
		}
	}

	void attack ()
	{
		if (Mathf.Abs (Input.GetAxis ("Fire1")) > 0.01f) {
			if (!attacking) {
				attacking = true;
				animator.SetTrigger ("attack");
			}
		} else {
			attacking = false;
		}
	}

	void move ()
	{
		float v = Input.GetAxis ("Horizontal") * currentVel;
		float speed = v < 0 ? v * -1 : v;
		animator.SetFloat ("speed", speed);
		Vector2 vel = new Vector2 (0, rbd.velocity.y);
		if (animator.GetCurrentAnimatorStateInfo (0).IsName (WALKING)) {
			currentVel = walkVel;
		} else if (animator.GetCurrentAnimatorStateInfo (0).IsName (RUNNING)) {
			currentVel = runVel;
		}
		vel.x = v * currentVel;
		rbd.velocity = vel;
		if (haciaDerecha && v < 0) {
			haciaDerecha = false;
			flip ();
		} else if (!haciaDerecha && v > 0) {
			haciaDerecha = true;
			flip ();
		}
	}

	void flip ()
	{
		var scale = transform.localScale;
		scale.x *= -1;
		transform.localScale = scale;
	}

	void OnTriggerEnter2D (Collider2D other)
	{
		string tag = other.gameObject.tag;
		if (tag == "ground") {
			onGround = true;
		}

		if (tag == "parchment") {
			ctrlParchment = other.gameObject.gameObject.GetComponent<ControlParchment> ();
			ctrlParchment.disappearParchment ();
			parchmentObtained++;
		}
		if (tag == "gem_ruby") {
			ctrlRuby = other.gameObject.gameObject.GetComponent<ControlRuby> ();
			ctrlRuby.disappearRuby ();
			numOfRubies++;
		}	


	}

	public void sustractEnergy(){
		energy--;
		if(energy <= 0){
			animator.SetTrigger ("dead");
		}
	}
}
