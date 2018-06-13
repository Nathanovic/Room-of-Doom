using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Boss_FSM;
using System.Collections;

//this script handles the AI attack state, using the Boss script
//can be used to get the closest hittable head of the ai's
public class BossManager : MonoBehaviour {

	private FSM fsm;

	public Transform[] targets;//the players
	public SpawnPositions spawner{ get; private set; }
	private LavaController lavaController;
	public float lavaRiseTime = 10;

	public static BossManager instance;
	public List<WormBase> worms = new List<WormBase>();
	public List<WormBase> phase2Worms;
	public int phase2ExtraWormCount = 4;//phase 3 = just all worms
	private int activeWormIndex;
	private WormBoss boss;

	public ShakeSettings wormDieCamShake;
	public ShakeSettings bossPhaseShake;

	private State[] gamePhases = new State[3];

	public List<float> regionHeights = new List<float>(3);

	public float winDelay = 1.5f;
	public event SimpleDelegate onGameWon;

	private void Awake(){
		if (instance != null) {
			Destroy (gameObject);
		} 
		else {
			instance = this;
			spawner = GetComponent<SpawnPositions> ();
		}
	}

	public void InitializeWorm(WormBase worm){
		if (worm is WormBoss) {
			boss = (WormBoss)worm;
			boss.onPhaseUp += StateUp;
			boss.onBossDied += OnBossDied;
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

		lavaController = GetComponentInChildren<LavaController> ();

		phase2Worms = new List<WormBase> ();
		for (int i = 0; i < phase2ExtraWormCount; i++) {
			phase2Worms.Add (worms [i]);
		}
	}

	public void DetachWormObject(Transform wormObj){
		wormObj.SetParent (transform);
	}

	#region FSM implementation
	private void Update(){
		fsm.Run ();
	}

	public void IntroPhase(){
		boss.WormUpdate ();
	}

	public void MultiWormPhase(){
		boss.WormUpdate ();
		foreach (WormBase worm in phase2Worms) {
			worm.WormUpdate ();
		}
	}

	public void StartFinalPhase(){
		StartCoroutine (LavaUp());
	}

	public void FinalPhase(){
		if (boss != null){
			boss.WormUpdate ();
		}
	
		foreach (WormBase worm in worms) {
			worm.WormUpdate ();
		}
	}

	private void StateUp(int phaseIndex){
		float intensity = phaseIndex == 2 ? 1f : 0f;
		foreach (WormBase worm in worms) {
			worm.intensity = intensity;
		}

		Shaker.instance.ControllerShake (bossPhaseShake);
		fsm.TriggerNextState (gamePhases [phaseIndex]);
	}
	#endregion

	private void OnWormDied(WormBase worm){
		worms.Remove (worm);
		Shaker.instance.CameraShake (wormDieCamShake);

		EvaluateWin ();
	}

	private IEnumerator LavaUp(){
		while (regionHeights.Count > 1) {
			lavaController.Rise (regionHeights.First ());
			regionHeights.RemoveAt (0);
			yield return new WaitForSeconds (lavaRiseTime);
		}
	}

	private void OnBossDied(){
		if (worms.Count == 0) {
			Shaker.instance.CameraShake (wormDieCamShake);			
			boss = null;
		}

		EvaluateWin ();
	}

	private void EvaluateWin(){
		if (worms.Count == 0 && boss == null) {
			StartCoroutine (DelayedWin ());
		}
	}

	private IEnumerator DelayedWin(){
		yield return new WaitForSeconds (winDelay);
		onGameWon ();
	}

	public List<Vector3> GetHeadPositions(Vector3 origin, float range){
		List<Vector3> availablePositions = new List<Vector3> ();

		TryAddHeadPos (origin, range, boss, ref availablePositions);
		foreach (WormBase worm in worms) {
			TryAddHeadPos (origin, range, worm, ref availablePositions);
		}

		return availablePositions;
	}

	private void TryAddHeadPos(Vector3 origin, float range, WormBase worm, ref List<Vector3> positions){
		Vector3 headPos = worm.GetHeadPosition ();
		if (Vector3.Distance (origin, headPos) <= range && headPos.y > 0f) {
			positions.Add (headPos);
		}
	}

	private void OnDrawGizmosSelected(){
		Gizmos.color = Color.red;

		for (int i = 0; i < regionHeights.Count; i++) {
			Vector3 regionCenter = new Vector3 (0f, regionHeights [i], 0f);
			Vector3 regionSize = new Vector3 (70f, 1f);
			Gizmos.DrawWireCube (regionCenter, regionSize);
		}
	}

	public Vector3 GetRandomTargetPos(){
		int rndmTargetIndex = Random.Range (0, targets.Length);
		return targets [rndmTargetIndex].position;
	}
}
