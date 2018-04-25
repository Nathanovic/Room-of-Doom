﻿using UnityEngine;

//the player implementation for combat
public class PlayerCombat : CharacterCombat {

	private PlayerBase baseScript;

	[Header("hit check values")]
	public LayerMask enemyLM;
	public Transform weaponCheck;

	[Header("weapon values")]
	public int weaponDamage;
	public float weaponRange;
	public float cooldown;
	public float remainingCooldown;

	private void Start(){
		baseScript = GetComponent<PlayerBase> ();
	}

	private void Update () {
		if (!baseScript.canControl)
			return;

		if (remainingCooldown == 0f && Input.GetButtonDown ("Attack")) {
			Attack ();
		}
		else if (remainingCooldown > 0f) {
			Cooldown ();
		}
	}

	//activate the cooldown and try to hit someone
	private void Attack(){
		remainingCooldown = cooldown;
		Collider2D other = Physics2D.OverlapCircle(weaponCheck.position, weaponRange, enemyLM);
        if (other != null) {
			CharacterCombat combatScript = other.GetComponent<CharacterCombat> ();
			combatScript.ApplyDamage (weaponDamage);
        }
	}

	private void Cooldown(){
		remainingCooldown -= Time.deltaTime;
		remainingCooldown = (remainingCooldown < 0f) ? 0f : remainingCooldown;
	}

	//draw a circle where the weapon can hit
	void OnDrawGizmos(){
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere (weaponCheck.position, weaponRange);
	}
}