using UnityEngine;

//used for the magmaworm movement
public class BezierCurve : MonoBehaviour {

	public Vector2[] points;
	public float calculationAccuracy = 15;

	public void DetachSelf(){
		transform.SetParent (null);
		transform.position = Vector3.zero;
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

	public void Reset(){
		points = new Vector2[]{
			new Vector2(1f, 0f),
			new Vector2(2f, 0f),
			new Vector2(3f, 0f)
		};
	}

	public Vector3 GetPoint(float t){
		Vector3 localPoint = GetLerpedPoint (points [0], points [1], points [2], t);
		return transform.TransformPoint (localPoint);
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
