using UnityEngine;
using Boss_FSM;

public class WormBoss : WormBase {

	private WormMovement worm;
	private CharacterCombat combatScript;
	private FSM fsm;
	private int state;

	public BossPhase[] bossPhases = new BossPhase[3];
	public delegate void BossPhaseDelegate(int stateIndex);
	public event BossPhaseDelegate onPhaseUp;

    // FMOD event emitter
    FMODUnity.StudioEventEmitter emitter;

    protected override void Start(){
		worm = GetComponent<WormMovement> ();
		worm.onAttackEnded += EvaluateState;
		
		base.Start ();

		State beginState = new BeginningState (this);
		bossPhases [0].Init (beginState);
		bossPhases [1].Init (new HardState (this));
		bossPhases [2].Init (new ImpossibleState (this));
		fsm = new FSM (beginState);

		combatScript = GetComponent<CharacterCombat> ();

        // Set the FMOD event emitter
        var target = GameObject.Find("Main Camera");
        emitter = target.GetComponent<FMODUnity.StudioEventEmitter>();
    }
	
	public override void WormUpdate(){
		fsm.Run ();
	}

	private void EvaluateState(){
        // Set the state parameter
        emitter.SetParameter("state", state);

		float hpPercentage = combatScript.HPPercentage ();
		if (bossPhases [state].CanStateUp (hpPercentage)) {
			state++;
			if (state == bossPhases.Length) {
				Debug.LogWarning ("game should be over now");
			}
			else {
				State nextState = bossPhases [state].nextState;
				fsm.TriggerNextState (nextState);
				onPhaseUp (state);

				EvaluateState ();
			}
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
	}
	public void ImpossibleState(){
		base.WormUpdate ();
	}
}

[System.Serializable]
public class BossPhase{
	[SerializeField]private float stateUpPercentage = 0.3f;

	public State nextState{ get; private set; }

	public void Init(State state){
		nextState = state;
	}

	public bool CanStateUp(float hpPercentage){
		return (nextState != null && hpPercentage <= stateUpPercentage);
	}
}
