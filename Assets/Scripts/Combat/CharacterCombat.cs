using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//basic version of combat, implements the IAttackable interface
//this script is used to make sure the player has health and can be hitted
public class CharacterCombat : MonoBehaviour, IAttackable {

	public int health = 5;
	private const float HITTED_IMMUNE_DURATION = 2f;
	private float remainingImmuneDuration = 0f;

	public delegate void HealthUpdateDelegate (int newHP);
	public event HealthUpdateDelegate onHealthChanged;
	public event SimpleDelegate onDie;

	public ParticleSystem hitPS;

	public bool customHealthBar;

	private void Awake(){
		CombatManager.Instance.RegisterPotentialTarget (this);
	}

	private void Start(){
		if (hitPS == null) {
			hitPS = transform.GetChild (1).GetComponent<ParticleSystem> ();
			if (hitPS == null)
				Debug.LogWarning ("hit ps not assigned???");
		}
	}

	protected virtual void Update(){
		if (remainingImmuneDuration > 0f) {
			remainingImmuneDuration -= Time.deltaTime;
		}
	}

	//apply damage to character; can only be done if health > 0
	public void ApplyDamage (int dmg, Vector3 hitPos){
		health -= dmg;
		remainingImmuneDuration = HITTED_IMMUNE_DURATION;
		if (health <= 0) {
			health = 0;
			if(onDie != null)
				onDie ();
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
		hitPS.transform.position = hitPos;
		hitPS.Play ();
	}

	public bool ValidTarget (){
		return health > 0 && remainingImmuneDuration <= 0f;
	}

	public Vector2 Position (){
		return (Vector2)transform.position;
	}
}
