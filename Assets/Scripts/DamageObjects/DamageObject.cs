using UnityEngine;

public class DamageObject : MonoBehaviour {

	private bool canDoDamage = true;
	public bool destroyOnCollision;

	public int damage;

	private void OnCollisionEnter2D(Collision2D coll){
		Collider2D other = coll.collider;
		TryDamageCollider (other);

		if (destroyOnCollision) {
			canDoDamage = false;
			Destroy (gameObject, 0.5f);
		}
	}

	private void OnTriggerStay2D(Collider2D other){
		TryDamageCollider (other);
	}

	private void TryDamageCollider(Collider2D other){
		if (other.tag == "Player" && canDoDamage) {
			CharacterCombat combatScript = other.GetComponent<CharacterCombat> ();
			if (combatScript.ValidTarget ()) {
				combatScript.ApplyDamage (damage);
				Destroy (gameObject);
			}
		}
	}
}
