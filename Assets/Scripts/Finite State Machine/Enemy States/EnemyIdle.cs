using System;
using Characters;
using Action = Characters.Enemies.Action;

namespace Finite_State_Machine.Enemy_States
{
    public class EnemyIdle : State
    {
        public EnemyIdle()
        {
            interval = 0f;
        }
        
        protected override void Initialize(MoveableObject agent) 
        {
            agent.SetAnimations(Action.Idle);
            ToComplete();
        }

        protected override void Executing(MoveableObject agent)
        {
            
        }

        protected override void Completed(MoveableObject agent)
        {
            
        }
    }
}