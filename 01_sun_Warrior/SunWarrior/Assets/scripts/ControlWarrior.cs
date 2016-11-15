using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ControlWarrior : MonoBehaviour
{
	const string pathTexturePotionOnTop = "potion_6";
	const string pathTextureParchmentOnTop = "scroll_5";
	const string pathTextureParchmentBlackOnTop = "scroll_5_black";

	static float walkVelocity = 3f;
	static float runVelocity = 3.5f;
	static string WALKING = "walking";
	static string RUNNING = "running";
	static string DYING = "Dying";
	static string HITTING = "Hitting";

	Rigidbody2D rbd;
	Animator animator;
	float currentVelocity = 2f;
	bool facingRight = true;
	bool attacking = false;

	ControlScene ctrlScene;

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
	public bool gameOver = false;

	public SpriteRenderer sprite;
	bool isFadeOut = false;
	public float timeToFadeIn = 1f;
	public float xHitForce = 300;
	Vector2 hitForce;

	public AudioClip warriorDeath;
	public AudioClip warriorJump;
	public AudioClip warriorAttck;
	public AudioClip warriorReciveHit;
	AudioSource aSource;

	//public Canvas canvas;


	// Use this for initialization
	void Start ()
	{
		rbd = GetComponent<Rigidbody2D> ();
		animator = GetComponent<Animator> ();
		sprite = GetComponent<SpriteRenderer> ();
		aSource = GetComponent<AudioSource> ();
		ctrlScene = GameObject.Find ("scene").GetComponent<ControlScene> ();

		/*canvas = GameObject.Find ("Canvas").GetComponent<Canvas> ();

		Image[] images = canvas.GetComponentsInChildren<Image> ();
		foreach (Image image in images) {
			image.enabled = false;
		}*/

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
		if (gameOver) {
			return;
		}

		onGround = Physics2D.OverlapCircle (groungCheck.position, groungRadious, whatIsGround);
		move ();

		jump ();

		attack ();

	}

	void jump ()
	{
		if (Input.GetAxis ("Jump") > 0.01f) {
			if (!jumping && onGround) {
				aSource.PlayOneShot (warriorJump);
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
				aSource.PlayOneShot (warriorAttck);
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

	public void sustractEnergy ()
	{
		if (!isFadeOut) {
			energy--;
			aSource.PlayOneShot (warriorReciveHit);
			if (energy > 0) {
				toDoWhenHitMe ();
			}
			isFadeOut = true;
			Invoke ("setFadeOutToFalse", timeToFadeIn);
			if (energy <= 0) {
				setFadeOutToFalse ();
				aSource.PlayOneShot (warriorDeath);
				animator.SetTrigger ("dead");
				ctrlScene.GameOver ();
				gameOver = true;
			}
		}
	}

	public bool hasWonTheGame (){
		return gameOver;
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

	public void addOneToNumOfRubies(){
		numOfRubies++;
	}

	public void addOneToParchmentObtained(){
		parchmentObtained++;
	}

	void OnGUI ()
	{
		drawEnergyHero ();
		drawParchmentFinded ();
	}

	void drawEnergyHero ()
	{
		Texture2D s = Resources.Load (pathTexturePotionOnTop, typeof(Texture2D)) as Texture2D;
		int x = 10;
		for (int i = 1; i <= energy; i++) {
			GUI.DrawTexture (new Rect (x, 10, 19, 32), s);
			x += 25;
		}
	}

	void drawParchmentFinded ()
	{
		
		Texture2D parchmentBlackOnTop = Resources.Load (pathTextureParchmentBlackOnTop, typeof(Texture2D)) as Texture2D;
		Texture2D parchmentOnTop = Resources.Load (pathTextureParchmentOnTop, typeof(Texture2D)) as Texture2D;
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

}
