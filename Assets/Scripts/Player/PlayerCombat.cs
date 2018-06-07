using UnityEngine;
using XInputDotNetPure;

//the player implementation for combat
public class PlayerCombat : CharacterCombat {

	private PlayerBase baseScript;
	private Animator anim;

	private PlayerIndex playerIndex;
	public ShakeSettings hittedControllerShakeSettings;
	public float remainingShakeDuration;

	[Header("hit check values")]
	public LayerMask enemyLM;
	public Transform weaponCheck;

	[Header("weapon values")]
	public int attackDamage;
	public float attackRange;

	public HealthBar hb;
	private int previousHealth;

    private void Start(){
		baseScript = GetComponent<PlayerBase> ();
		anim = GetComponentInChildren<Animator> ();
		hb.Init (this, false);

		playerIndex = (PlayerIndex)(GetComponent<PlayerInput> ().controllerNumber - 1);

		previousHealth = base.health;
		base.onHealthChanged += OnHealthChanged;
		base.onDie += OnDie;
	}

	protected override void Update () {
		base.Update ();
	}

	//triggered by animation
	public void TryDoDamage(){
		Collider2D other = Physics2D.OverlapCircle(weaponCheck.position, attackRange, enemyLM);
		if (other != null) {
			RaycastHit2D hit = Physics2D.Raycast (transform.position, (other.transform.position - transform.position), attackRange * 2f, enemyLM);
			IAttackable combatScript = other.GetComponent<IAttackable> ();
			if (combatScript.ValidTarget ()) {
				combatScript.ApplyDamage (attackDamage, hit.point, (Vector3)hit.point - transform.transform.position);
			}
		}
	}

	private void OnHealthChanged (int newHP){
		if (newHP < previousHealth) {
			remainingShakeDuration = hittedControllerShakeSettings.duration;
		}

		previousHealth = newHP;
	}

	private void FixedUpdate(){
		if (remainingShakeDuration > 0f) {
			remainingShakeDuration -= Time.fixedDeltaTime;

			if (remainingShakeDuration > 0f) {
				GamePad.SetVibration (playerIndex, hittedControllerShakeSettings.amount, 0f);
			}
			else {
				GamePad.SetVibration (playerIndex, 0f, 0f);
			}
		}
	}

	private void OnDie(){
		anim.SetTrigger ("die");
		anim.SetBool ("dead", true);		
	}

	//draw a circle where the weapon can hit
	void OnDrawGizmos(){
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere (weaponCheck.position, attackRange);
	}
}
