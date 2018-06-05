using UnityEngine;

namespace Boss_FSM{
	public class FSM {

		private State currentState;

		public FSM(State startState){
			TriggerNextState (startState);
		}

		public void TriggerNextState(State nextState){
			if (currentState != null) {
				currentState.ExitState ();
			}

			nextState.NextState ();
			currentState = nextState;
		}

		public void Run(){
			currentState.RunState ();
		}
	}
}
