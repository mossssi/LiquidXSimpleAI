using LiquidX.Enemy;

namespace LiquidX.SM
{
    public abstract class StateBase
    {
        protected StateMachine _stateMachine;
        protected Guard _guard;

        public abstract void EnterState();
        public abstract void ExecuteState();
        public abstract void ExitState();

        public StateBase(StateMachine stateMachine)
        {
            _stateMachine = stateMachine;
            _guard = stateMachine.Guard;
        }
    }
}