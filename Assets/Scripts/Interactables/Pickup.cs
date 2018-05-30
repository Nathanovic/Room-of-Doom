using UnityEngine;

//a pickup is a item that can be picked up by the player
public class Pickup : MonoBehaviour {

	private Rigidbody2D rb;
	private Collider2D coll;
	private SpriteRenderer visual;

	//some callback should be build here to prevent double pickup-ing

	private void Start(){
		rb = GetComponent<Rigidbody2D> ();
		coll = GetComponent<Collider2D> ();
		visual = GetComponent<SpriteRenderer> ();
	}

	public void PickMeUp(){
		rb.isKinematic = true;
		coll.enabled = false;
		visual.enabled = false;

		rb.velocity = Vector2.zero;
	}

	public void DropMe(Vector3 dropPosition){
		rb.isKinematic = false;
		coll.enabled = true;
		visual.enabled = true;

		transform.position = dropPosition;
	}
}
