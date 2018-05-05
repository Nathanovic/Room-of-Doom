using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//basic version of combat, implements the IAttackable interface
//this script is used to make sure the player has health and can be hitted
public class CharacterCombat : MonoBehaviour, IAttackable {

	public int health = 5;

	public delegate void HealthUpdateDelegate (int newHP);
	public event HealthUpdateDelegate onHealthChanged;

	public ParticleSystem hitPS;

	private void Awake(){
		CombatManager.Instance.RegisterPotentialTarget (this);
	}

	//apply damage to character; can only be done if health > 0
	public void ApplyDamage (int dmg, float otherX){
		health -= dmg;
		if (health <= 0) {
			health = 0;
		}

		//make sure all related systems know that the health changed:
		if (onHealthChanged != null) {
			onHealthChanged (health);
		}

		//make sure we cannot be attacked anymore:
		if (health == 0) {
			CombatManager.Instance.PotentialTargetDied (this);
		}

		//show the player visually that we have been hit:
		hitPS.Play ();
	}

	public bool ValidTarget (){
		return health > 0;
	}

	public Vector2 Position (){
		return (Vector2)transform.position;
	}

	public Vector2 PrevPosition (){
		throw new System.NotImplementedException ();
	}
}
