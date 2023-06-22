using UnityEngine;
using LiquidX.Enemy;
using LiquidX.Perception;

namespace LiquidX.SM
{
    public class StateMachine : MonoBehaviour
    {
        // private members
        private StateBase _currentState;
        private StateFactory _stateFactory;
        private Guard _guard;
        private AIPerception _perception;

        // properties
        public StateBase CurrentState => _currentState;
        public Guard Guard => _guard;
        public AIPerception Perception => _perception;

		public void Initialize(Guard guard)
        {
            _guard = guard;
            _stateFactory = new StateFactory(this);
            ChangeState(_stateFactory.Patrolling());
            _perception = GetComponent<AIPerception>();
            _perception.OnPlayerFound += PlayerFound;
            _perception.OnPlayerLost += PlayerLost;
        }

        private void Update()
        {
            if (_currentState != null)
            {
                _currentState.ExecuteState();
            }
        }

        public void ChangeState(StateBase newState)
        {
            if (newState == null) return;

            if (_currentState != null)
            {
                _currentState.ExitState();
            }

            // change to new state
            _currentState = newState;
            _currentState.EnterState();
        }

        private void PlayerFound(Player player)
        {
            ChangeState(_stateFactory.Chasing());
        }

        private void PlayerLost()
        {
            
		}

        public void SetPatrolling()
        {
			ChangeState(_stateFactory.Patrolling());
		}

		private void OnDrawGizmos()
		{
			if(_currentState != null)
            {
                _currentState.OnDrawGizmos();
            }
		}

		private void OnDisable()
		{
			_perception.OnPlayerFound -= PlayerFound;
			_perception.OnPlayerLost -= PlayerLost;
		}
	}
}