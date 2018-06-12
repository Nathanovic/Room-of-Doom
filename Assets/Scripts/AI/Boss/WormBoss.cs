using UnityEngine;
using Boss_FSM;

public class WormBoss : WormBase {

	private WormMovement worm;
	private CharacterCombat combatScript;
	private SpawnPositions spawner;
	private FSM fsm;
	private int state;

	public BossPhase[] bossPhases = new BossPhase[3];
	public delegate void BossPhaseDelegate(int stateIndex);
	public event BossPhaseDelegate onPhaseUp;

	public float phaseUpDuration = 3f;
	private bool phaseIncreasedUndergroundTime;

	public SimpleDelegate onBossDied;

  // FMOD parameter trigger
  private FMODUnity.StudioParameterTrigger parameterTrigger;

	protected override void Start(){
		worm = GetComponent<WormMovement> ();
		combatScript = GetComponent<CharacterCombat> ();
		worm.onAttackEnded += EvaluateState;
		combatScript.onDie += OnBossWormDied;

		base.Start ();

		State beginState = new BeginningState (this);
		bossPhases [0].Init (beginState);
		bossPhases [1].Init (new HardState (this));
		bossPhases [2].Init (new ImpossibleState (this));
		fsm = new FSM (beginState);

		spawner = BossManager.instance.spawner;
		spawner.SetSpawnMethod (bossPhases [0].SpawnInCameraView ());

    // Set the FMOD parameter trigger
    parameterTrigger = GetComponent<FMODUnity.StudioParameterTrigger> ();
	}

	public override void WormUpdate(){
		fsm.Run ();
	}

	private void EvaluateState(){
		float hpPercentage = combatScript.HPPercentage ();
		if (hpPercentage == 0f) {
			StateUp ();
		}
	}

	private void StateUp(){
		state++;
		Debug.Log ("new state: " + state);
		if (state < bossPhases.Length) {
			State nextState = bossPhases [state].nextState;
			fsm.TriggerNextState (nextState);
			onPhaseUp (state);
			phaseIncreasedUndergroundTime = true;
			spawner.SetSpawnMethod (bossPhases [state].SpawnInCameraView ());

			EvaluateState ();
		}
	}

	private void OnBossWormDied(){
		onBossDied ();
	}

	protected override void OnAttackEnded () {
		base.OnAttackEnded ();
		if (phaseIncreasedUndergroundTime) {
			phaseIncreasedUndergroundTime = false;
			undergroundTime = phaseUpDuration;
		}
	}

	public void BeginningState(){
		base.WormUpdate ();
	}

	public void TriggerHardState(){
		intensity = 0.6f;
		GetComponent<Healer> ().Heal ();
	}
	public void HardState(){
		base.WormUpdate ();
	}

	public void TriggerImpossibleState(){
		intensity = 1f;
		GetComponent<Healer> ().Heal ();
	}
	public void ImpossibleState(){
		base.WormUpdate ();
	}
}

[System.Serializable]
public class BossPhase{
	[SerializeField]private bool spawnInCameraView;

	public State nextState{ get; private set; }

	public void Init(State state){
		nextState = state;
	}

	public bool SpawnInCameraView(){
		return spawnInCameraView;
	}
}
