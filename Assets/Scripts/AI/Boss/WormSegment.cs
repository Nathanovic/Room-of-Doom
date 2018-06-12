using UnityEngine;

public class WormSegment : MonoBehaviour, IAttackable {

	private CharacterCombat myCombat;//used to damage the worm
	private WormBase wormScript;

	private SpriteRenderer colorElement;
	public Transform prevSegment;

	public float length;
	public float moveT{get;private set;}
	private float curveMoveSpeed;
	private int id;
	public static int segmentID;

	private void Start(){
		myCombat = transform.parent.GetComponent<CharacterCombat> ();
		wormScript = transform.parent.GetComponent<WormBase> ();
	}

	public void Prepare(Transform otherSegment, float beforeSegmentDelay, float speed){
		segmentID++;
		id = segmentID;
		prevSegment = otherSegment;//.GetChild(0);
		moveT = -beforeSegmentDelay;
		curveMoveSpeed = speed;
	}

	public void TraverseCurve(IWormTraverseable line){
		moveT += curveMoveSpeed * Time.deltaTime;
		transform.position = line.GetPoint (moveT);

		Vector3 targetPos = prevSegment.position;line.GetPoint (moveT + 0.1f);
		Vector3 dir = targetPos - transform.position;
		float angle = Mathf.Atan2(dir.y,dir.x) * Mathf.Rad2Deg + 180;
		transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

		Debug.DrawLine (transform.position, targetPos, new Color(0f, 0f, 0.1f * id));
	}
		
	private void OnTriggerEnter2D(Collider2D other){
		wormScript.TryHitOther (other);
	} 

	public void ApplyDamage (int dmg, Vector3 hitPos, Vector3 hitDir){
		myCombat.ApplyDamage (dmg, hitPos, hitDir);
	}

	public bool ValidTarget (){
		return myCombat.health > 0f;
	}

	public Vector2 Position (){
		return (Vector2)transform.position;
	}

	public Rigidbody2D Detach(Color deadColor){
		transform.SetParent (null);
		GetComponent<SpriteRenderer> ().color = deadColor;
		return gameObject.AddComponent<Rigidbody2D> ();
	}

	public void RecolorSegment(Color newColor){
		if (colorElement == null) {
			colorElement = transform.GetComponentInChildren<SpriteRenderer> ();
		}

		colorElement.color = newColor;
	}
}
