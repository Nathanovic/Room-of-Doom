using UnityEngine;
using System.Linq;

//used for the magmaworm movement
public class AttackCurveBehaviour : MonoBehaviour, IWormTraverseable {

	public LayerMask groundLM;

	private WormMovement moveScript;
	private Vector2[] points;
	private float calculationAccuracy;

	private void Start(){
		calculationAccuracy = 15;
		moveScript = GetComponent<WormMovement> ();
	}

	public void Prepare(Vector3 startPos, Vector3 enemyPos){
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
			targetPos.y = -2f;

		points = new Vector2[] {
			startPos,
			controlPoint,
			targetPos
		};
	}

	public float GetLength (){
		float curveLength = 0f;
		Vector2 lineStart = GetPoint (0f);
		for (int i = 1; i <= calculationAccuracy; i++) {
			float t = (float)i / calculationAccuracy;
			Vector2 lineEnd = GetPoint (t);
			curveLength += Vector2.Distance (lineStart, lineEnd);
			lineStart = lineEnd;
		}

		return curveLength;
	}

	public void Run(WormSegment[] wormSegments){
		foreach (WormSegment segment in wormSegments) {
			segment.TraverseCurve (this);
		}

		if (wormSegments.Last ().moveT >= 1f) {
			moveScript.EndAttack ();
		}
	}

	public Vector3 GetPoint(float t){
		Vector3 lerpedPoint = GetLerpedPoint (points [0], points [1], points [2], t);
		return lerpedPoint;
	}

	public bool SpawnCentered(){
		return false;
	}

	private Vector3 GetLerpedPoint(Vector3 p0, Vector3 p1, Vector3 p2, float t){
		t = Mathf.Clamp01 (t);
		float oneMinusT = 1f - t;
		Vector3 point = oneMinusT * oneMinusT * p0 +
			2f * oneMinusT * t * p1 +
			t * t * p2;

		return point;
	}
}
