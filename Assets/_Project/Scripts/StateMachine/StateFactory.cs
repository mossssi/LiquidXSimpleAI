using LiquidX.SM.States;

namespace LiquidX.SM
{
	public class StateFactory
	{
		private StateMachine _stateMachine;

		private PatrolState _patrolState;
		private ChaseState _chaseState;

		public StateFactory(StateMachine stateMachine)
		{
			_stateMachine = stateMachine;
		}

		public PatrolState Patrolling()
		{
			if (_patrolState == null)
			{
				_patrolState = new PatrolState(_stateMachine);
			}
			return _patrolState;
		}

		public ChaseState Chasing()
		{
			if (_chaseState == null)
			{
				_chaseState = new ChaseState(_stateMachine);
			}
			return _chaseState;
		}
	}
}