using Characters;

namespace Finite_State_Machine.States
{
    public class Idle : State
    {
        public override void Execute(MoveableObject agent)
        {
            agent.StopAnimation();
        }
    }
}