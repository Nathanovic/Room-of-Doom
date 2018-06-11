using UnityEngine;

public class RockFall : MonoBehaviour {

	public GameObject rockPrefab;

	public float minX, maxX, y;

	public int rockAmount = 4;
	public float minRockFallTime = 0.5f;
	public float maxRockFallTime = 7f;

	private void Start(){
		StartSpawning ();
	}

	public void StartSpawning(){
		for(int i = 0; i < rockAmount; i ++){
			SpawnRandomNew ();
		}		
	}

	private void SpawnRandomNew(){
		float rndmTime = Random.Range (minRockFallTime, maxRockFallTime);
		Invoke ("SpawnRock", rndmTime);		
	}

	private void SpawnRock(){
		Vector3 rockPos = new Vector3 (Random.Range (minX, maxX), y);
		GameObject rock = GameObject.Instantiate (rockPrefab, rockPos, Quaternion.identity);

		SpawnRandomNew ();
	}

	void OnDrawGizmosSelected(){
		Gizmos.color = Color.grey;
		Vector3 size = new Vector3 (maxX - minX, 1f, 1f);
		Vector3 center = new Vector3 (size.x * 0.5f + minX, y, 1f);
		Gizmos.DrawCube (center, size);
	}
}
