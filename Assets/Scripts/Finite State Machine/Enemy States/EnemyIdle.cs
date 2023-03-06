using Characters;
using Characters.Enemy;

namespace Finite_State_Machine.Enemy_States
{
    public class EnemyIdle : State
    {
        public EnemyIdle()
        {
            interval = 0f;
        }
        public override void Execute(MoveableObject agent)
        {
            agent.SetAnimations(Action.Idle);
        }
    }
}