using UnityEngine;

//the player implementation for combat
public class PlayerCombat : CharacterCombat {

	private PlayerInput input;
	private PlayerBase baseScript;
	private Animator anim;

	[Header("hit check values")]
	public LayerMask enemyLM;
	public Transform weaponCheck;

	[Header("weapon values")]
	public int attackDamage;
	public float attackRange;
	public float cooldown;
	public float remainingCooldown;

	private void Start(){
		baseScript = GetComponent<PlayerBase> ();
		anim = GetComponentInChildren<Animator> ();
		input = GetComponent<PlayerInput> ();
	}

	private void Update () {
		if (!baseScript.canControl)
			return;

		if (remainingCooldown == 0f && input.ButtonIsDown(PlayerInput.Button.RB)) {
			Attack ();
		}
		else if (remainingCooldown > 0f) {
			Cooldown ();
		}
	}

	//activate the cooldown and try to hit someone
	private void Attack(){
		remainingCooldown = cooldown;
		Collider2D other = Physics2D.OverlapCircle(weaponCheck.position, attackRange, enemyLM);
        if (other != null) {
			CharacterCombat combatScript = other.GetComponent<CharacterCombat> ();
			combatScript.ApplyDamage (attackDamage, transform.position.x);
        }

		anim.SetTrigger ("stab");
	}

	private void Cooldown(){
		remainingCooldown -= Time.deltaTime;
		remainingCooldown = (remainingCooldown < 0f) ? 0f : remainingCooldown;
	}

	//draw a circle where the weapon can hit
	void OnDrawGizmos(){
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere (weaponCheck.position, attackRange);
	}
}
