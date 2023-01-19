using Characters;
using Characters.Enemy;

namespace Finite_State_Machine
{
    public class State
    {
        protected enum StateStatus
        {
            Initialize,
            Executing,
            Completed,
            Failed
        }

        protected StateStatus currentStatus = StateStatus.Initialize;
    
        public virtual void Execute(MoveableObject agent)
        {
            
        }
    }
}