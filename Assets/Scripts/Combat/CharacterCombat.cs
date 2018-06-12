using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//basic version of combat, implements the IAttackable interface
//this script is used to make sure the player has health and can be hitted
public class CharacterCombat : MonoBehaviour, IAttackable {

	public int maxHealth{ get; private set; }
	public int health = 5;
	public float hittedImmuneDuration = 2f;//2f for players, 0 for AI
	private float remainingImmuneDuration = 0f;

	public delegate void HealthUpdateDelegate (int newHP);
	public delegate void MaxHealthUpdateDelegate (int newHP, Color newColor);
	public event MaxHealthUpdateDelegate onMaxHealthChanged;
	public event HealthUpdateDelegate onHealthChanged;
	public event SimpleDelegate onDie;

	public ParticleSystem hitPS;

	public bool customHealthBar;

	public ShakeSettings hittedShakeSettings;

	private int lives;

	private void Awake(){
		CombatManager.Instance.RegisterPotentialTarget (this);
		maxHealth = health;
	}

	private void Start(){
		if (hitPS == null) {
			hitPS = transform.GetChild (1).GetComponent<ParticleSystem> ();
			if (hitPS == null)
				Debug.LogWarning ("hit ps not assigned???");
		}

		Healer healScript = GetComponent<Healer> ();
		if (healScript != null) {
			lives = healScript.healCount;
			healScript.onHeal += OnHeal;
		}
	}

	protected virtual void Update(){
		if (remainingImmuneDuration > 0f) {
			remainingImmuneDuration -= Time.deltaTime;
		}
	}

	//apply damage to character; can only be done if health > 0
	public void ApplyDamage (int dmg, Vector3 hitPos, Vector3 hitDir){
		ApplyDamage (dmg);

		//show the player visually that we have been hit:
		hitDir.y = 0f;
		hitDir.Normalize ();
		hitDir.y = hitDir.z = 1f;
		hitPS.transform.position = hitPos;
		hitPS.transform.localScale = hitDir;
		hitPS.Play ();
	}

	//simplified version (without hitPS)
	public void ApplyDamage(int dmg){
        health -= dmg;
		remainingImmuneDuration = hittedImmuneDuration;
		if (health <= 0) {
			health = 0;
		}

        HealthChangedEvent();

		//make sure we cannot be attacked anymore:
		if (health == 0 && lives == 0) {
			if(onDie != null) {
				onDie ();
			}
			CombatManager.Instance.PotentialTargetDied (this);
		}

		//shake the camera
		Shaker.instance.CameraShake(hittedShakeSettings, dmg);
	}

    public void HealthChangedEvent(){
        if (onHealthChanged != null){
            onHealthChanged(health);
        }
    }

	public bool ValidTarget (){
		return health > 0 && remainingImmuneDuration <= 0f;
	}

	public Vector2 Position (){
		return (Vector2)transform.position;
	}

	public float HPPercentage(){
		return (float)health / maxHealth;
	}

	public void MakeImmune(float duration){
		remainingImmuneDuration = duration;
	}

	public void PrepareRevive(int newMaxHP, Color newColor = default(Color)){
		health = 0;
		maxHealth = newMaxHP;		
		onMaxHealthChanged (newMaxHP, newColor);
	}

	public void HealUp(float hpPercentage){
		health = Mathf.RoundToInt ((float)maxHealth * hpPercentage);
		if (onHealthChanged != null) {
			onHealthChanged (health);
		}
	}

	private void OnHeal(){
		lives--;
	}
}
