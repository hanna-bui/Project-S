using System;
using Characters;

namespace Finite_State_Machine.States
{
    public class PlayerIdle : State
    {

        public PlayerIdle()
        {
            interval = 0f;
        }
        
        public override void Execute(MoveableObject agent)
        {
            switch (CurrentStatus)
            {
                case StateStatus.Initialize:
                    agent.StopAnimation();
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