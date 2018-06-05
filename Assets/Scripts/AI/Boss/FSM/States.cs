using UnityEngine;

namespace Boss_FSM{
	
	public abstract class State{
		public virtual void NextState (){}
		public abstract void RunState ();
		public virtual void ExitState(){}
	}

	#region boss states
	public class BeginningState : State{
		private WormBoss boss;

		public BeginningState(WormBoss worm){
			boss = worm;
		}

		public override void RunState () {
			boss.BeginningState ();
		}
	}

	public class HardState : State{
		private WormBoss boss;

		public HardState(WormBoss worm){
			boss = worm;
		}

		public override void NextState (){
			boss.TriggerHardState ();
		}

		public override void RunState () {
			boss.HardState ();
		}
	}

	public class ImpossibleState : State{
		private WormBoss boss;

		public ImpossibleState(WormBoss worm){
			boss = worm;
		}

		public override void RunState () {
			boss.ImpossibleState ();
		}
	}
	#endregion
}