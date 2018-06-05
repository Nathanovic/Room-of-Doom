using UnityEngine;

namespace Boss_FSM{
	
	public abstract class State{
		public virtual void Enter (){}
		public abstract void Run ();
		public virtual void Exit(){}
	}

	#region boss states
	public class BeginningState : State{
		private WormBoss boss;

		public BeginningState(WormBoss worm){
			boss = worm;
		}

		public override void Run () {
			boss.BeginningState ();
		}
	}

	public class HardState : State{
		private WormBoss boss;

		public HardState(WormBoss worm){
			boss = worm;
		}

		public override void Enter (){
			boss.TriggerHardState ();
		}

		public override void Run () {
			boss.HardState ();
		}
	}

	public class ImpossibleState : State{
		private WormBoss boss;

		public ImpossibleState(WormBoss worm){
			boss = worm;
		}

		public override void Enter (){
			boss.TriggerImpossibleState ();
		}

		public override void Run () {
			boss.ImpossibleState ();
		}
	}
	#endregion
}