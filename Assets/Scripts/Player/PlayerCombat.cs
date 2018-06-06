﻿using UnityEngine;

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

	public HealthBar hb;

    private void Start(){
		baseScript = GetComponent<PlayerBase> ();
		anim = GetComponentInChildren<Animator> ();
		input = GetComponent<PlayerInput> ();
		hb.Init (this, false);
		base.onHealthChanged += OnHealthChanged;

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
		if (newHP == 0) {
			anim.SetTrigger ("die");
			anim.SetBool ("dead", true);
		}
	}

	//draw a circle where the weapon can hit
	void OnDrawGizmos(){
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere (weaponCheck.position, attackRange);
	}
}
