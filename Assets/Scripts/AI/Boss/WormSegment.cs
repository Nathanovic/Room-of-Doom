using UnityEngine;

public class WormSegment : MonoBehaviour {

	public float length;
	private float moveT = 0f;
	private float curveMoveSpeed;

	public void Prepare(float beforeSegmentDelay, float speed){
		moveT = -beforeSegmentDelay;
		curveMoveSpeed = speed;
	}

	public void TraverseCurve(BezierCurve curve){
		moveT += curveMoveSpeed * Time.deltaTime;
		transform.position = curve.GetPoint (moveT);
	}

	public bool KeepTraversing(){
		return moveT < 1f;	
	}
}
