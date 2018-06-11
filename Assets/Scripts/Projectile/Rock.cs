using UnityEngine;

public class Rock : MonoBehaviour {

	private bool canDoDamage = true;

	public int damage;

	private void Start () {
	}

	private void OnCollisionEnter2D(Collision2D coll){
		Collider2D other = coll.collider;
		Debug.Log ("boulder vs " + other.name + "_candoDamage: " + canDoDamage);
		if (other.tag == "Player" && canDoDamage) {
			CharacterCombat combatScript = other.GetComponent<CharacterCombat> ();
			if (combatScript.ValidTarget ()) {
				combatScript.ApplyDamage (damage);
				Destroy (gameObject);
			}
		}

		canDoDamage = false;
		Destroy (gameObject, 0.5f);
	}
}
