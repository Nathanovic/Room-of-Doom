using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//handle the movement of the magma worm
//the body exists out of segments, and each segments just follows the head
//handles the magma worm attacks:
//attacks the targets it has by changing the moveCurve.points
public class WormMovement : MonoBehaviour {

	public WormSegment[] wormSegments{ get; private set; }
	public LayerMask groundLM;

	public float startDelay = 1f;
	private float delay = 0f;
	private Transform head;
	public float headLength;
	private BezierCurve movementCurve;
	private float moveT = 0f;
	private float curveMoveSpeed;
	public float movementSpeed = 2f;

	private void Start(){
		head = transform.GetChild (0);

		movementCurve = GetComponentInChildren<BezierCurve> ();
		movementCurve.DetachSelf ();
		wormSegments = GetComponentsInChildren<WormSegment> ();
	}

	public void CurveAttackUpdate(){
		if (delay > 0f) {
			delay -= Time.deltaTime;
		} 
		else {
			moveT += curveMoveSpeed * Time.deltaTime;
			transform.position = movementCurve.GetPoint (moveT);

			Vector3 dir = movementCurve.GetPoint (moveT + 0.1f) - transform.position;
			float angle = Mathf.Atan2(dir.y,dir.x) * Mathf.Rad2Deg + 180;
			head.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

			foreach (WormSegment segment in wormSegments) {
				segment.TraverseCurve (movementCurve);
			}
		}
	}

	public void StartCurveAttack(Vector3 startPos, Vector3 enemyPos){
		//calculate a control point
		Vector3 controlPoint = Vector3.Lerp (startPos, enemyPos, 0.7f);
		float xDist = Mathf.Abs (startPos.x - enemyPos.x);
		controlPoint.y += xDist * 1.5f;

		//make sure our targetPos is beneath the ground surface:
		Vector3 targetPos = enemyPos;
		Vector3 enemyDir = (enemyPos - controlPoint);
		RaycastHit2D[] hits = Physics2D.RaycastAll(enemyPos, enemyDir, 20f, groundLM);
		foreach (RaycastHit2D hit in hits) {
			if (hit.collider != null && hit.point.y < 0.3f) {
				targetPos = hit.point + (Vector2)enemyDir.normalized * 2.5f;
				Debug.DrawLine (controlPoint, targetPos, Color.yellow, 2f);
				break;
			}
		}

		if (targetPos.y > 0f)
			targetPos.y = -1.5f;

		movementCurve.points [0] = startPos;
		movementCurve.points [1] = controlPoint;
		movementCurve.points [2] = targetPos;

		//debug visually:
		Debug.DrawLine (startPos, controlPoint, Color.red, 1f);
		Debug.DrawLine (controlPoint, targetPos, Color.red, 1f);

		float moveCurveLength = movementCurve.GetLength ();
		curveMoveSpeed = movementSpeed / moveCurveLength;

		Transform prevSegment = head;
		float beforeSegmentDelay = headLength / moveCurveLength;
		foreach (WormSegment segment in wormSegments) {
			segment.Prepare (prevSegment, beforeSegmentDelay, curveMoveSpeed);
			beforeSegmentDelay += segment.length / moveCurveLength;
			prevSegment = segment.transform;
		}

		moveT = 0f;
		delay = startDelay;
	}
}
