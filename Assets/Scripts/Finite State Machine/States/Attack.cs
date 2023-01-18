using Characters;

namespace Finite_State_Machine.States
{

    public class Attack : State
    {
        public override void Execute(MoveableObject agent)
        {
            var gm = agent.gm;
            var grid = gm.grid;
            
            if (currentStatus is StateStatus.Initialize)
            {
                
                currentStatus = StateStatus.Executing;
            }
            
            if (currentStatus is StateStatus.Executing)
            {
                currentStatus = StateStatus.Completed;
            }

            if (currentStatus is StateStatus.Completed)
            {
                agent.ChangeState(new Idle());
            }
        }
    }
}