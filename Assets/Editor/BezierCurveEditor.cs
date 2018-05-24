using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(BezierCurve))]
public class BezierCurveEditor : Editor {

	private BezierCurve curve;
	private Transform handleTransform;
	private Quaternion handleRotation;

	private const int LINE_STEPS = 15;

	private void OnSceneGUI(){
		curve = target as BezierCurve;
		handleTransform = curve.transform;
		handleRotation = Tools.pivotRotation == PivotRotation.Local ? handleTransform.rotation : Quaternion.identity;

		Vector2 p0 = ShowPoint (0);
		Vector2 p1 = ShowPoint (1);
		Vector2 p2 = ShowPoint (2);

		Handles.color = Color.white;
		Handles.DrawLine (p0, p1);
		Handles.DrawLine (p1, p2);

		Color handleColor = new Color (1f, 0f, 0f);
		Handles.color = handleColor;
		Vector3 lineStart = curve.GetPoint (0f);
		for (int i = 1; i <= LINE_STEPS; i++) {
			float t = (float)i / LINE_STEPS;
			Vector3 lineEnd = curve.GetPoint (t);

			handleColor = new Color (1f-t, 0f, 0f);
			Handles.color = handleColor;
			Handles.DrawLine (lineStart, lineEnd);
			lineStart = lineEnd;
		}
	}

	//showpoint is 3d because the handles are in 3d orientation
	private Vector3 ShowPoint (int index) {
		Vector3 point = handleTransform.TransformPoint(curve.points[index]);
		EditorGUI.BeginChangeCheck ();
		point = Handles.DoPositionHandle (point, handleRotation);
		if (EditorGUI.EndChangeCheck ()) {
			Undo.RecordObject (curve, "Move Point");
			EditorUtility.SetDirty (curve);
			curve.points [index] = handleTransform.InverseTransformPoint (point);
		}

		return point;
	}
}
