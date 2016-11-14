using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEditor;

public class ControlWarrior : MonoBehaviour
{
	static float walkVelocity = 3f;
	static float runVelocity = 3.5f;
	static string WALKING = "walking";
	static string RUNNING = "running";
	static string DYING = "Dying";
	static string HITTING = "Hitting";

	const string pathTexturePotionOnTop = "Assets/Premios/PNG/64px/potion_6.png";
	const string pathTextureParchmentOnTop = "Assets/Premios/PNG/64px/scroll_5.png";
	const string pathTextureParchmentBlackOnTop = "Assets/Premios/PNG/64px/scroll_5_black.png";

	Rigidbody2D rbd;
	Animator animator;
	float currentVelocity = 2f;
	bool facingRight = true;
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

	public bool onGround = false;
	public Transform groungCheck;
	float groungRadious = 0.2f;
	public LayerMask whatIsGround;

	public bool allowJump;
	public string numCollition;
	public bool died = false;

	public SpriteRenderer sprite;
	bool isFadeOut = false;
	public float timeToFadeIn = 1f;
	public float xHitForce = 300;
	Vector2 hitForce;
	bool isHit = false;

	//public Canvas canvas;


	// Use this for initialization
	void Start ()
	{
		rbd = GetComponent<Rigidbody2D> ();
		animator = GetComponent<Animator> ();
		sprite = GetComponent<SpriteRenderer> ();

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
		if (isFadeOut) {
			FadeOut ();
		}
	}
		
	// Update is called once per frame
	void FixedUpdate ()
	{
		if (animator.GetCurrentAnimatorStateInfo (0).IsName (DYING)) {
			return;
		}

		onGround = Physics2D.OverlapCircle (groungCheck.position, groungRadious, whatIsGround);
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
			if (!jumping && onGround) {
				
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

		if (!animator.GetCurrentAnimatorStateInfo (0).IsName (HITTING)) {
			float move = Input.GetAxis ("Horizontal") * currentVelocity;

			//float speed = move < 0 ? move * -1 : move;
			animator.SetFloat ("speed", Mathf.Abs (move));
			if (animator.GetCurrentAnimatorStateInfo (0).IsName (WALKING)) {
				currentVelocity = walkVelocity;
			} else if (animator.GetCurrentAnimatorStateInfo (0).IsName (RUNNING)) {
				currentVelocity = runVelocity;
			}
			rbd.velocity = new Vector2 (move * currentVelocity, rbd.velocity.y);
			if (facingRight && move < 0) {
				flip ();
			} else if (!facingRight && move > 0) {
				flip ();
			}
		}
	}

	void flip ()
	{
		facingRight = !facingRight;
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
			if (!ctrlParchment.isDisappear ()) {
				ctrlParchment.disappearParchment ();
				parchmentObtained++;
			}
			
		}
		if (tag == "gem_ruby") {
			ctrlRuby = other.gameObject.gameObject.GetComponent<ControlRuby> ();
			if (!ctrlRuby.isDisappear ()) {
				ctrlRuby.disappearRuby ();
				numOfRubies++;	
			}
		}	


	}

	public void sustractEnergy ()
	{
		if (!isFadeOut) {
			energy--;
			if (energy > 0) {
				toDoWhenHitMe ();
			}
			isFadeOut = true;
			Invoke ("setFadeOutToFalse", timeToFadeIn);
			if (energy <= 0) {
				setFadeOutToFalse ();
				animator.SetTrigger ("dead");
				died = true;
			}
		}
	}

	public bool isDead (){
		return died;
	}
		
	void FadeOut() {
		float a = Mathf.PingPong (Time.time / 0.05f, 1.0f);
		sprite.color = new Color(1f, 1f, 1f, a);
	}

	void FadeIn() {
		sprite.color = new Color(1f, 1f, 1f, 1f);
	}

	void setFadeOutToFalse(){
		isFadeOut = false;
		FadeIn ();
	}

	void toDoWhenHitMe(){
		animator.SetTrigger ("hit");
		rbd.AddForce(Vector2.left * xHitForce);
	}
}
