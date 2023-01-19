using Characters;

namespace Finite_State_Machine.States
{
    public class PlayerIdle : State
    {
        public override void Execute(MoveableObject agent)
        {
            agent.StopAnimation();
        }
    }
}