using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SpawnPositions))]
public class SpawnPositionsEditor : Editor {

	private SpawnPositions positionScript;
	private Transform handleTransform;
	private Quaternion handleRotation;

	private void OnSceneGUI(){
		positionScript = target as SpawnPositions;
		handleTransform = positionScript.transform;
		handleRotation = Tools.pivotRotation == PivotRotation.Local ? handleTransform.rotation : Quaternion.identity;

		for (int i = 0; i < positionScript.points.Length; i++) {
			ShowPoint (i);
		}
	}

	//showpoint is 3d because the handles are in 3d orientation
	private Vector3 ShowPoint (int index) {
		Vector3 point = (positionScript.points[index]);
		EditorGUI.BeginChangeCheck ();
		point = Handles.DoPositionHandle (point, handleRotation);
		if (EditorGUI.EndChangeCheck ()) {
			Undo.RecordObject (positionScript, "Move Point");
			EditorUtility.SetDirty (positionScript);
			positionScript.points [index] = (point);
		}

		return point;
	}
}
