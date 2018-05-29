using System.Collections.Generic;
using UnityEngine;
using System.Linq;

//this script handles the boss state
//can be used to get the closest hittable head of the ai's
public class BossManager : MonoBehaviour {

	public static BossManager instance;
	private List<MagmaWorm> worms = new List<MagmaWorm>();

	private void Awake(){
		if (instance != null) {
			Destroy (gameObject);
		} 
		else {
			instance = this;
		}
	}

	public void InitializeBoss(MagmaWorm boss){
		worms.Add (boss);
	}

	public List<Vector3> GetHeadPositions(Vector3 origin, float range){
		List<Vector3> availablePositions = new List<Vector3> ();

		foreach (MagmaWorm worm in worms) {
			Vector3 headPos = worm.GetHeadPosition ();
			if (Vector3.Distance (origin, headPos) <= range) {
				availablePositions.Add (headPos);
			}
		}

		return availablePositions;
	}
}
