using UnityEngine;
using Boss_FSM;

public class WormBoss : WormBase {

	private WormMovement worm;
	private Healer healScript;
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

  // FMOD Event emitter
  public FMODUnity.StudioEventEmitter musicEventEmitter;

	protected override void Start(){
		worm = GetComponent<WormMovement> ();
		healScript = GetComponent<Healer> ();
		combatScript = GetComponent<CharacterCombat> ();
		worm.onAttackEnded += EvaluateState;
		combatScript.onDie += OnBossWormDied;

		base.Start ();

		State beginState = new BeginningState (this);
		bossPhases [0].Init (beginState);
		bossPhases [1].Init (new HardState (this));
		bossPhases [2].Init (new ImpossibleState (this));
		fsm = new FSM (beginState);
		healScript.Heal (bossPhases [0].phaseHealth, bossPhases [0].bossColor);

		combatScript.onDie -= OnDie;
		spawner = BossManager.instance.spawner;
		spawner.SetSpawnMethod (bossPhases [0].SpawnInCameraView ());
	}

	public override void WormUpdate(){
		fsm.Run ();
	}

	private void EvaluateState(){
		Debug.Log ("evaluate state");
		float hpPercentage = combatScript.HPPercentage ();
		if (hpPercentage == 0f) {
			StateUp ();
		}
	}

	private void StateUp(){
		state++;

    musicEventEmitter.SetParameter("state",state);

		if (state < bossPhases.Length) {
			BossPhase phase = bossPhases [state];
			State nextState = phase.nextState;
			fsm.TriggerNextState (nextState);
			onPhaseUp (state);
			phaseIncreasedUndergroundTime = true;
			spawner.SetSpawnMethod (phase.SpawnInCameraView ());
			healScript.Heal (phase.phaseHealth, phase.bossColor);

			foreach (WormSegment segment in wormSegments) {
				
			}
		} 
		else {
			Debug.Log ("worm boss dies");
			OnDie ();	
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
	}
	public void HardState(){
		base.WormUpdate ();
	}

	public void TriggerImpossibleState(){
		intensity = 1f;
	}
	public void ImpossibleState(){
		base.WormUpdate ();
	}
}

[System.Serializable]
public class BossPhase{
	[SerializeField]private bool spawnInCameraView;
	public int phaseHealth;
	public Color bossColor;

	public State nextState{ get; private set; }

	public void Init(State state){
		nextState = state;
	}

	public bool SpawnInCameraView(){
		return spawnInCameraView;
	}
}
