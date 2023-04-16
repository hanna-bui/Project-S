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
        
        protected override void Initialize(MoveableObject agent) 
        {
            agent.StopAnimation();
            ChangeStatus(StateStatus.Completed);
        }

        protected override void Executing(MoveableObject agent)
        {
            
        }

        protected override void Completed(MoveableObject agent)
        {
            
        }
    }
}