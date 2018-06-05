using System.Collections.Generic;
using UnityEngine;
using System.Linq;

//this script handles the AI attack state, using the Boss script
//can be used to get the closest hittable head of the ai's
public class BossManager : MonoBehaviour {

	public static BossManager instance;
	private List<WormBase> worms = new List<WormBase>();

	private void Awake(){
		if (instance != null) {
			Destroy (gameObject);
		} 
		else {
			instance = this;
		}
	}

	public void InitializeBoss(WormBase boss){
		worms.Add (boss);
	}

	public List<Vector3> GetHeadPositions(Vector3 origin, float range){
		List<Vector3> availablePositions = new List<Vector3> ();

		foreach (WormBase worm in worms) {
			Vector3 headPos = worm.GetHeadPosition ();
			if (Vector3.Distance (origin, headPos) <= range && headPos.y > 0f) {
				availablePositions.Add (headPos);
			}
		}

		return availablePositions;
	}

	public enum BossPhase{
		start,
		heavy
	}

	[System.Serializable]
	public class BossPhaseData{
		public float phaseStartThreshold = 0.5f;
	}
}
