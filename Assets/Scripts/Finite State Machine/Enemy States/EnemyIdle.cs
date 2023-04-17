using System;
using Characters;
using Characters.Enemies;

namespace Finite_State_Machine.Enemy_States
{
    public class EnemyIdle : State
    {
        public EnemyIdle()
        {
            interval = 0f;
        }
        
        protected override void Initialize(Agent agent) 
        {
            agent.SetExactAnimation(0);
            ToComplete();
        }

        protected override void Executing(Agent agent)
        {
            
        }

        protected override void Completed(Agent agent)
        {
            
        }
    }
}