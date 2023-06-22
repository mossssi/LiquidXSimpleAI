using UnityEngine;
using LiquidX.Enemy;

namespace LiquidX.SM
{
    public class StateMachine : MonoBehaviour
    {
        // private members
        private StateBase _currentState;
        private Guard _guard;
        private StateFactory _stateFactory;

        // properties
        public StateBase CurrentState => _currentState;
        public Guard Guard => _guard;

        public void Initialize(Guard guard)
        {
            _guard = guard;
            _stateFactory = new StateFactory(this);
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
    }
}