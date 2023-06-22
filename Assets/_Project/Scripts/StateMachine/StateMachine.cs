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
            _perception = GetComponent<AIPerception>();
            
            // Actions which will be called on detecting or losing the player
            _perception.OnPlayerFound += OnPlayerFound;
            _perception.OnPlayerLost += OnPlayerLost;
            _perception.OnSuspect += OnSuspect;

            // Initial state
            ChangeState(_stateFactory.Patrolling());
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

        private void OnPlayerFound(Player player)
        {
            // When player is found chase it immediately 
            ChangeState(_stateFactory.Chasing());
        }

        private void OnPlayerLost()
        {
            // TODO: Maybe another state for losing player
		}

        // Calls on hearing noise without seeing player
        private void OnSuspect(Vector3 position)
        {
            var state = _stateFactory.Searching();
            state.SetDestination(position);
			ChangeState(state);
        }

        /// <summary>
        /// For set patrolling from anywhere
        /// </summary>
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
            // Removing listeners on disable for unwanted behaviours
			_perception.OnPlayerFound -= OnPlayerFound;
			_perception.OnPlayerLost -= OnPlayerLost;
			_perception.OnSuspect -= OnSuspect;
		}
	}
}