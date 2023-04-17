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
        
        protected override void Initialize(Agent agent) 
        {
            agent.StopAnimation();
            StateProgress();
            StateProgress();
        }

        protected override void Executing(Agent agent)
        {
            
        }

        protected override void Completed(Agent agent)
        {
            
        }
    }
}