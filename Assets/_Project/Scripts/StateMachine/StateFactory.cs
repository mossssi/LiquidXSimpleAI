using LiquidX.SM.States;

namespace LiquidX.SM
{
	public class StateFactory
	{
		private StateMachine _stateMachine;

		private PatrolState _patrolState;

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
	}
}