using UnityEngine;
using System.Collections.Generic;
using System.Linq;

//used for spawning stuff like where the magma worm should pop up
//the positions are world relative!
public class SpawnPositions : MonoBehaviour {

	public Vector3[] points = new Vector3[3];

	private Camera mainCam;
	private bool spawnInCameraField;
	public float minAwayDist = 5f;

	private void Start(){
		mainCam = Camera.main;
	}

	public Vector3 GetRandomPoint(SpawnSettings settings){
		List<Vector3> camPoints = new List<Vector3> ();

		if (spawnInCameraField && !settings.spawnCentered) {
			float camX = mainCam.transform.position.x;
			float camWidthHalf = mainCam.orthographicSize * mainCam.aspect;
			float minX = camX - camWidthHalf;
			float maxX = camX + camWidthHalf;
			Debug.DrawRay (new Vector3 (minX, 0f), Vector3.up * 10, Color.red, 1f);
			Debug.DrawRay (new Vector3 (maxX, 0f), Vector3.up * 10, Color.red, 1f);
				
			camPoints = (from point in points
			             where point.x > minX && point.x < maxX
			             select point).ToList ();
		}
		else if (settings.spawnCentered) {
			Vector3 point = points [0];
			point.x = mainCam.transform.position.x;
			return point;
		}
		else {
			camPoints = (from point in points
				where Mathf.Abs(point.x - settings.targetPos.x) >= minAwayDist
				select point).ToList();
		}

		int rndmIndex = Random.Range (0, camPoints.Count);
		return camPoints [rndmIndex];
	}

	public void SetSpawnMethod(bool spawnInCamField){
		spawnInCameraField = spawnInCamField;
	}
}
	
[System.Serializable]
public class SpawnSettings{
	public bool spawnCentered;
	public Vector3 targetPos;
}
