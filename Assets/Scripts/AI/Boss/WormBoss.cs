using UnityEngine;
using Boss_FSM;

public class WormBoss : WormBase {

	private WormMovement worm;
	private CharacterCombat combatScript;
	private FSM fsm;
	private int state;

	public BossPhase[] bossPhases = new BossPhase[3];

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

	//first evaluate state, then think about when to get out of the ground
	protected override void HandleLastSegment(WormSegment lastSegment){
		lastSegment.onFinishedMoving += EvaluateState;
		base.HandleLastSegment (lastSegment);
	}
	
	protected override void Update(){
		fsm.Run ();
	}

	private void EvaluateState(){
		float hpPercentage = combatScript.HPPercentage ();
		if (bossPhases [state].CanStateUp (hpPercentage)) {
			state++;
			if (state == bossPhases.Length) {
				Debug.LogWarning ("game should be over now");
			}
			else {
				State nextState = bossPhases [state].nextState;
				fsm.TriggerNextState (nextState);
				Debug.Log ("trigger state " + nextState.ToString ());
			}
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
		base.Update ();
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
