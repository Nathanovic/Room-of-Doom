using UnityEngine;

public class WormSegment : MonoBehaviour, IAttackable {

	private CharacterCombat myCombat;//used to damage the worm
	private MagmaWorm wormScript;

	public Transform prevSegment;
	public float length;
	private float moveT = 0f;
	private float curveMoveSpeed;
	private int id;
	public static int segmentID;

	private void Start(){
		myCombat = transform.parent.GetComponent<CharacterCombat> ();
		wormScript = transform.parent.GetComponent<MagmaWorm> ();
	}

	public void Prepare(Transform otherSegment, float beforeSegmentDelay, float speed){
		segmentID++;
		id = segmentID;
		prevSegment = otherSegment;//.GetChild(0);
		moveT = -beforeSegmentDelay;
		curveMoveSpeed = speed;
	}

	public void TraverseCurve(BezierCurve curve){
		moveT += curveMoveSpeed * Time.deltaTime;
		transform.position = curve.GetPoint (moveT);

		Vector3 targetPos = prevSegment.position;curve.GetPoint (moveT + 0.1f);
		Vector3 dir = targetPos - transform.position;
		float angle = Mathf.Atan2(dir.y,dir.x) * Mathf.Rad2Deg + 180;
		transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

		Debug.DrawLine (transform.position, targetPos, new Color(0f, 0f, 0.1f * id));
	}

	public bool KeepTraversing(){
		return moveT < 1f;	
	}
		
	private void OnTriggerEnter2D(Collider2D other){
		wormScript.TryHitOther (other);
	} 

	public void ApplyDamage (int dmg, Vector3 hitPos)
	{
		myCombat.ApplyDamage (dmg, hitPos);
	}

	public bool ValidTarget ()
	{
		return myCombat.health > 0f;
	}

	public Vector2 Position ()
	{
		return (Vector2)transform.position;
	}
}
