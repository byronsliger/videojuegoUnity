using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ControlZombie : MonoBehaviour
{
	public float defaultVelocity = -1f;
	Rigidbody2D rbd;
	Animator animator;
	Canvas canvasZombie;
	static string WALKING = "walking";
	static string IDLING = "idling";
	static string ATTACKING = "attacking";

	const int SCRAP_OBJECTS = 13;

	public Transform hero;
	bool isNegative = true;
	bool needFlip = true;

	public ControlWarrior ctrlWarrior;
	public int currentEnergy = 10;
	int startEnergy = 10;
	bool isDead = false;

	public Slider healthEnemy;
	public Image fillImage;
	Color zeroHealthColor = Color.red;
	Color fullHealthColor = Color.green;
	public float distance;

	public int maxDistanceToFollow = 20;

	public AudioClip zombieDeath;
	public AudioClip zombieAttack;
	public AudioClip zombieCatchWarrior;
	public AudioClip zombieReciveHit;
	AudioSource aSource;
	bool hasSoundedCatchWarrior = false;

	void OnGUI ()
	{
		SetHealthUI ();
		canvasZombie = healthEnemy.GetComponentInParent<Canvas> ();
	}

	// Use this for initialization
	void Start ()
	{
		rbd = GetComponent<Rigidbody2D> ();
		animator = GetComponent<Animator> ();
		ctrlWarrior = hero.gameObject.GetComponent<ControlWarrior> ();
		aSource = GetComponent<AudioSource> ();
	}
	
	// Update is called once per frame
	void FixedUpdate ()
	{
		if (isDead || ctrlWarrior.hasWonTheGame ()) {
			return;
		}
		distance = Vector2.Distance (transform.position, hero.position);
		walk ();
		doAnimation ();
	}

	void walk ()
	{
		if (animator.GetCurrentAnimatorStateInfo (0).IsName (WALKING)) {
			float velocity = defaultVelocity;
			if (distance < maxDistanceToFollow) {
				if (!hasSoundedCatchWarrior) {
					aSource.PlayOneShot (zombieCatchWarrior);
					hasSoundedCatchWarrior = true;
				}
				Vector2 dir = (hero.transform.position - transform.position).normalized;
				velocity = dir.x * 2f;
				followHero (velocity);
			} else {
				hasSoundedCatchWarrior = false;
				if (Random.value < 1f / (60f * 2f)) {
					isNegative = !isNegative;
					flip (true);
				}
			}
			rbd.velocity = new Vector2 (velocity, 0);

		}
	}

	void followHero (float velocity)
	{
		if (velocity < 0 && !isNegative) {
			needFlip = true;
			isNegative = true;
		} else if (velocity >= 0 && isNegative) {
			needFlip = true;
			isNegative = false;
		} else {
			needFlip = false;
		}
		if (needFlip) {
			flip ();
		}
	}

	void doAnimation ()
	{
		bool walking = animator.GetCurrentAnimatorStateInfo (0).IsName (WALKING);
		bool idling = animator.GetCurrentAnimatorStateInfo (0).IsName (IDLING);
		bool attacking = animator.GetCurrentAnimatorStateInfo (0).IsName (ATTACKING);
		if ((walking || attacking) && Random.value < 1f / (60f * 7f)) {
			animator.SetTrigger ("idle");
		} else if (!attacking && Random.value < 1f / (60f * 7f)) {
			animator.SetTrigger ("attack");
			aSource.PlayOneShot (zombieAttack);
		} else if ((idling || attacking) && Random.value < 1f / (60f * 1f)) {
			animator.SetTrigger ("walk");
		} 
	}

	void flip (bool isRamdonFlip = false)
	{
		defaultVelocity *= -1;
		var scale = transform.localScale;
		scale.x *= -1;
		transform.localScale = scale;
		if (!isRamdonFlip)
			needFlip = false;
	}

	public void sustractEnergy ()
	{
		currentEnergy--;
		aSource.PlayOneShot (zombieReciveHit);
		if (currentEnergy <= 0 && !isDead) {
			aSource.PlayOneShot (zombieDeath);
			animator.SetTrigger ("dead");
			gameObject.layer = SCRAP_OBJECTS;
			canvasZombie.enabled = false;
			isDead = true;
		}
	}

	void SetHealthUI ()
	{
		healthEnemy.value = currentEnergy;

		fillImage.color = Color.Lerp (zeroHealthColor, fullHealthColor, currentEnergy / startEnergy);
	}
}
