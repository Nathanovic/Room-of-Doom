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

	#region bossmanager states
	public class GameStartPhase : State{
		private BossManager manager;

		public GameStartPhase(BossManager _manager){
			manager = _manager;
		}

		public override void Run () {
			manager.IntroPhase ();
		}
	}

	public class MultiWormPhase : State{
		private BossManager manager;

		public MultiWormPhase(BossManager _manager){
			manager = _manager;
		}

		public override void Run () {
			manager.MultiWormPhase ();
		}
	}

	public class FinalPhase : State{
		private BossManager manager;

		public FinalPhase(BossManager _manager){
			manager = _manager;
		}

		public override void Enter (){
			manager.StartFinalPhase ();
		}

		public override void Run () {
			manager.FinalPhase ();
		}
	}
	#endregion
}