using UnityEngine;

public class Laser : MonoBehaviour {

	public LayerMask playerLM;
	private Vector2 size;
	private int damage;

	private LineRenderer laserBeam;
	private BoxCollider2D laserBox;

	public void Init(int dmg){
		damage = dmg;
		playerLM = LayerMask.GetMask ("Player");
		laserBeam = GetComponent<LineRenderer> ();
		laserBox = GetComponent<BoxCollider2D> ();
		Disable ();
	}

	public void Enable(Vector2 laserOffset, Vector2 laserSize, Vector3[] laserPoints){
		laserBeam.enabled = true;
		laserBox.enabled = true;

		laserBox.offset = laserOffset;
		laserBox.size = laserSize;
		laserBeam.SetPositions (laserPoints);
	}

	public void Disable(){
		laserBeam.enabled = false;
		laserBox.enabled = false;
	}

	private void OnTriggerEnter2D(Collider2D other){
		Debug.Log ("hit: " + other.name + "_" + other.tag);
		if (other.tag == "Player") {
			Debug.Log ("hit player!!!");
			CharacterCombat combatScript = other.GetComponent<CharacterCombat> ();
			if (combatScript.ValidTarget ()) {
				combatScript.ApplyDamage (damage);					
			}
		}
	}
}
