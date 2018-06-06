using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Boss_FSM;

//this script handles the AI attack state, using the Boss script
//can be used to get the closest hittable head of the ai's
public class BossManager : MonoBehaviour {

	private FSM fsm;

	public static BossManager instance;
	public List<WormBase> worms = new List<WormBase>();
	private WormBoss boss;

	public CameraShakeSettings wormDeadShake;

	private State[] gamePhases = new State[3];

	public float[] regionHeights = new float[3];

	private void Awake(){
		if (instance != null) {
			Destroy (gameObject);
		} 
		else {
			instance = this;
		}
	}

	public void InitializeWorm(WormBase worm){
		if (worm is WormBoss) {
			boss = (WormBoss)worm;
			boss.onPhaseUp += StateUp;
		}
		else {
			worm.onWormDied += OnWormDied;
			worms.Add (worm);			
		}
	}

	private void Start(){
		gamePhases[0] = new GameStartPhase (this);
		gamePhases[1] = new MultiWormPhase (this);
		gamePhases[2] = new FinalPhase (this);
		fsm = new FSM (gamePhases[0]);
	}

	public void DetachWormObject(Transform wormObj){
		wormObj.SetParent (transform);
	}

	#region FSM implementation
	private void Update(){
		fsm.Run ();
	}

	public void StartPhase(){
		boss.WormUpdate ();
	}

	public void MultiWormPhase(){
		boss.WormUpdate ();
		foreach (WormBase worm in worms) {
			worm.WormUpdate ();
		}
	}

	public void FinalPhase(){
		boss.WormUpdate ();
		foreach (WormBase worm in worms) {
			worm.WormUpdate ();
		}
	}

	private void StateUp(int phaseIndex){
		float intensity = phaseIndex == 2 ? 1f : 0f;
		foreach (WormBase worm in worms) {
			worm.intensity = intensity;
		}
		fsm.TriggerNextState (gamePhases [phaseIndex]);
	}
	#endregion

	private void OnWormDied(WormBase worm){
		worms.Remove (worm);
		CameraShake.instance.Shake (wormDeadShake);
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

	private void OnDrawGizmos(){
		Gizmos.color = Color.red;

		for (int i = 0; i < regionHeights.Length; i++) {
			Vector3 regionCenter = new Vector3 (0f, regionHeights [i], 0f);
			Vector3 regionSize = new Vector3 (30f, 1f);
			Gizmos.DrawWireCube (regionCenter, regionSize);
		}
	}

	[System.Serializable]
	public class BossPhaseData{
		public float phaseStartThreshold = 0.5f;
	}
}
