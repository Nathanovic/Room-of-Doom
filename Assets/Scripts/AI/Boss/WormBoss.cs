using UnityEngine;
using Boss_FSM;
using System.Linq;

public class WormBoss : WormBase {

	private WormMovement worm;
	private Healer healScript;
	private CharacterCombat combatScript;
	private SpawnPositions spawner;
	private FSM fsm;
	private int state;

	private SpriteRenderer headColorElement;
	public UnityEngine.UI.Image headUIElement;
	private Material laserMat;

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

		combatScript.onDie -= OnDie;
		spawner = BossManager.instance.spawner;

		headColorElement = head.GetChild (0).GetComponent<SpriteRenderer> ();
		laserMat = GetComponent<AttackLineBehaviour> ().laserObj.
			GetComponentInChildren<Renderer> ().material;
		ApplyBossChanges (bossPhases [0]);
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

			ApplyBossChanges (phase);
		} 
		else {
			Debug.Log ("worm boss dies");
			OnDie ();	
		}
	}

	private void ApplyBossChanges(BossPhase phase){
		spawner.SetSpawnMethod (phase.SpawnInCameraView ());
		healScript.Heal (phase.phaseHealth, phase.bossColor);		
		foreach (WormSegment segment in wormSegments) {
			segment.RecolorSegment (phase.bossColor);
		}
		headColorElement.color = phase.bossColor;
		headUIElement.color = phase.bossColor;
		Color matC = phase.bossColor;
		matC.a = 0.5f;
		laserMat.SetColor("_TintColor", matC);
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
