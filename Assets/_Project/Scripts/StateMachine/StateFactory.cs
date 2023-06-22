using LiquidX.SM.States;
using UnityEngine;

namespace LiquidX.SM
{
	// This is a basic factory pattern for making objects in one place
	public class StateFactory
	{
		private StateMachine _stateMachine;

		private PatrolState _patrolState;
		private ChaseState _chaseState;
		private SearchState _searchState;

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

		public SearchState Searching()
		{
			if (_searchState == null)
			{
				_searchState = new SearchState(_stateMachine);
			}
			return _searchState;
		}
	}
}