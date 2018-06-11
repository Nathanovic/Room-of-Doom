using UnityEngine;

public class Laser : MonoBehaviour {

	public int damage;

	private void OnTriggerEnter2D(Collider2D other){
		if (other.tag == "Player") {
			CharacterCombat combatScript = other.GetComponent<CharacterCombat> ();
			if (combatScript.ValidTarget ()) {
				combatScript.ApplyDamage (damage);					
			}
		}
	}
}
