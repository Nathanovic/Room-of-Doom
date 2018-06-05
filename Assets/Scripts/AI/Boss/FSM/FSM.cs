using UnityEngine;

namespace Boss_FSM{
	public class FSM {

		private State currentState;

		public FSM(State startState){
			TriggerNextState (startState);
		}

		public void TriggerNextState(State nextState){
			if (currentState != null) {
				currentState.Exit ();
			}

			nextState.Enter ();
			currentState = nextState;
		}

		public void Run(){
			currentState.Run ();
		}
	}
}
