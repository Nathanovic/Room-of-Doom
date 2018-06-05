using UnityEngine;
using Boss_FSM;

public class WormBoss : WormBase {

	private WormMovement worm;
	private CharacterCombat combatScript;
	private FSM fsm;
	private int state;

	public BossPhase[] bossPhases = new BossPhase[2];

	protected override void Start(){
		base.Start ();

		State beginState = new BeginningState (this);
		bossPhases [0].Init (beginState);
		bossPhases [1].Init (new HardState (this));
		bossPhases [2].Init (new ImpossibleState (this));
		fsm = new FSM (beginState);

		worm = GetComponent<WormMovement> ();
		combatScript = GetComponent<CharacterCombat> ();
	}

	protected override void HandleLastSegment(WormSegment lastSegment){
		lastSegment.onFinishedMoving += EvaluateState;
	}
	
	protected override void Update(){
		fsm.Run ();
	}

	private void EvaluateState(){
		float hpPercentage = combatScript.HPPercentage ();
		if (bossPhases [state].CanStateUp (hpPercentage)) {
			State nextState = bossPhases [state].nextState;
			fsm.TriggerNextState (nextState);
			state++;
		}
	}

	public void BeginningState(){
		base.Update ();
	}

	public void TriggerHardState(){
		intensity = 0.8f;
	}

	public void HardState(){
		base.Update ();
		Debug.Log ("man that's hard!");
	}

	public void TriggerImpossibleState(){
		intensity = 1f;
	}

	public void ImpossibleState(){
		Debug.Log ("impossiblee hard");
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
}
