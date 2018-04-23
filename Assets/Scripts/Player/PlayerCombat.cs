using UnityEngine;

//the player implementation for combat
public class PlayerCombat : CharacterCombat {

	//[SerializeField]
	public int weaponDamage;
	public int weaponRange;
	public float cooldown;
	public float remainingCooldown;

	private void Update () {
		if (remainingCooldown == 0f && Input.GetButtonDown ("Attack")) {
			Attack ();
		}
		else if (remainingCooldown > 0f) {
			Cooldown ();
		}
	}

	private void Attack(){
		remainingCooldown = cooldown;
	}

	private void Cooldown(){
		remainingCooldown -= Time.deltaTime;
		remainingCooldown = (remainingCooldown < 0f) ? 0f : remainingCooldown;
	}
}
