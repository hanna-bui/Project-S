using System;
using Characters;
using Action = Characters.Enemy.Action;

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
            switch (CurrentStatus)
            {
                case StateStatus.Initialize:
                    agent.SetAnimations(Action.Idle);
                    ChangeStatus(StateStatus.Completed);
                    break;
                case StateStatus.Executing:
                    break;
                case StateStatus.Completed:
                    break;
                case StateStatus.Failed:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}