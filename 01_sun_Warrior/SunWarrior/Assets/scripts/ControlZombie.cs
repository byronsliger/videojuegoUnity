using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ControlZombie : MonoBehaviour
{
	public float vel = -1;
	Rigidbody2D rbd;
	Animator animator;
	static string WALKING = "walking";
	static string IDLING = "idling";
	static string ATTACKING = "attacking";
	static string DYING = "dying";

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

	void OnGUI ()
	{
		SetHealthUI ();
	}

	// Use this for initialization
	void Start ()
	{
		rbd = GetComponent<Rigidbody2D> ();
		animator = GetComponent<Animator> ();
		ctrlWarrior = hero.gameObject.GetComponent<ControlWarrior> ();
	}
	
	// Update is called once per frame
	void FixedUpdate ()
	{
		if (isDead) {
			return;
		}
		walk ();
		doAnimation ();
	}

	void walk ()
	{
		if (animator.GetCurrentAnimatorStateInfo (0).IsName (WALKING)) {
			Vector2 dir = (hero.transform.position - transform.position).normalized;
			Vector2 vector = new Vector2 (dir.x, 0);
			rbd.velocity = vector;
			if (dir.x < 0 && !isNegative) {
				needFlip = true;
				isNegative = true;
			} else if (dir.x >= 0 && isNegative) {
				needFlip = true;
				isNegative = false;
			} else {
				needFlip = false;
			}
			if (needFlip) {
				flip ();
			}
		}
	}

	void doAnimation ()
	{
		bool walking = animator.GetCurrentAnimatorStateInfo (0).IsName (WALKING);
		bool idling = animator.GetCurrentAnimatorStateInfo (0).IsName (IDLING);
		bool attacking = animator.GetCurrentAnimatorStateInfo (0).IsName (ATTACKING);
		if ((walking || attacking) && Random.value < 1f / (60f * 14f)) {
			animator.SetTrigger ("idle");
		} else if ((idling || attacking) && Random.value < 1f / (60f * 3f)) {
			animator.SetTrigger ("walk");
		} else if (!attacking && Random.value < 1f / (60f * 5f)) {
			animator.SetTrigger ("attack");
		}
	}

	void flip ()
	{
		vel *= -1;
		var scale = transform.localScale;
		scale.x *= -1;
		transform.localScale = scale;
		needFlip = false;
	}

	public void sustractEnergy ()
	{
		currentEnergy--;
		if (currentEnergy <= 0 && !isDead) {
			animator.SetTrigger ("dead");
			gameObject.layer = SCRAP_OBJECTS;
			isDead = true;
		}
	}

	void SetHealthUI ()
	{
		healthEnemy.value = currentEnergy;

		fillImage.color = Color.Lerp (zeroHealthColor, fullHealthColor, currentEnergy / startEnergy);
	}
}
