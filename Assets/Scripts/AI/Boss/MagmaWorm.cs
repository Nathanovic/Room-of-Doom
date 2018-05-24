using UnityEngine;
using System.Collections.Generic;

//handle the movement of the magma worm
//the body exists out of segments, and each segments just follows the head
//handles the magma worm attacks:
//attacks the targets it has by changing the moveCurve.points
public class MagmaWorm : MonoBehaviour {

	public Transform[] targets;//the players
	private SpawnPositions spawner;

	private WormSegment[] wormSegments;
	private WormSegment lastSegment;

	public float startDelay = 1f;
	private float delay = 0f;
	public float headLength;
	private BezierCurve movementCurve;
	private float moveT = 0f;
	private float curveMoveSpeed;
	public float movementSpeed = 2f;

	private Vector3 deactivePosition;
	private bool curveAttack;

	public int attackDamage;
	public float minXAttackDist = 4f;

	private void Start(){
		deactivePosition = transform.position;

		movementCurve = GetComponentInChildren<BezierCurve> ();
		spawner = GetComponentInChildren<SpawnPositions> ();
		movementCurve.DetachSelf ();
		wormSegments = GetComponentsInChildren<WormSegment> ();
		lastSegment = wormSegments[wormSegments.Length - 1];
	}

	private void StartCurveAttack(){
		transform.position = deactivePosition;

		int rndmTargetIndex = Random.Range (0, targets.Length);
		Vector3 targetPos = targets [rndmTargetIndex].position;

		//choose a random start pos that is far away from it:
		List<Vector3> availableSpawnPositions = new List<Vector3>();
		Vector3 startPos = Vector3.zero;
		int i = 0;
		foreach (Vector3 point in spawner.points) {
			float dist = Mathf.Abs (point.x - targetPos.x);
			if (dist > minXAttackDist) {
				availableSpawnPositions.Add (point);
			}
			i++;
		}

		Debug.Log (availableSpawnPositions.Count + " positions available!");
		int rndmIndex = Random.Range (0, availableSpawnPositions.Count);
		startPos = availableSpawnPositions [rndmIndex];

		//calculate a control point
		Vector3 controlPoint = Vector3.Lerp (startPos, targetPos, 0.7f);
		float xDist = Mathf.Abs (startPos.x - targetPos.x);
		controlPoint.y += xDist * 1.5f;

		movementCurve.points [0] = startPos;
		movementCurve.points [1] = controlPoint;
		movementCurve.points [2] = targetPos;

		//debug visually:
		Debug.DrawLine (startPos, controlPoint, Color.red, 1f);
		Debug.DrawLine (controlPoint, targetPos, Color.red, 1f);

		float moveCurveLength = movementCurve.GetLength ();
		curveMoveSpeed = movementSpeed / moveCurveLength;

		float beforeSegmentDelay = headLength / moveCurveLength;
		foreach (WormSegment segment in wormSegments) {
			segment.Prepare (beforeSegmentDelay, curveMoveSpeed);
			beforeSegmentDelay += segment.length / moveCurveLength;
		}

		moveT = 0f;
		delay = startDelay;
		curveAttack = true;
	}

	private void Update(){
		if (curveAttack) {
			if (delay > 0f) {
				delay -= Time.deltaTime;
			} 
			else {
				moveT += curveMoveSpeed * Time.deltaTime;
				transform.position = movementCurve.GetPoint (moveT);
				foreach (WormSegment segment in wormSegments) {
					segment.TraverseCurve (movementCurve);
				}

				if (!lastSegment.KeepTraversing ()) {
					curveAttack = false;
				}
			}
		}

		if (Input.GetKeyUp (KeyCode.Space) && targets.Length > 0) {
			StartCurveAttack ();
		}
	}

	private void OnTriggerEnter2D(Collider2D other){
		if (other.tag == "Player") {
			CharacterCombat target = other.GetComponent<CharacterCombat> ();
			if (target.ValidTarget ()) {
				target.ApplyDamage (attackDamage, transform.position.x);
			}
		}
	}
}
