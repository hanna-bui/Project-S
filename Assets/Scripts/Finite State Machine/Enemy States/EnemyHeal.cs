using System;
using Characters;
using Characters.Enemy;
using JetBrains.Annotations;
using Action = Characters.Enemy.Action;
// ReSharper disable PossibleNullReferenceException

namespace Finite_State_Machine.Enemy_States
{
    public class EnemyHeal : State
    {
        private const float DefaultInterval = 1f;
        
        public override void Execute([NotNull] MoveableObject agent)
        {
            agent = agent as Enemy;
            switch (CurrentStatus)
            {
                case StateStatus.Initialize:
                    agent.SetAnimations(Action.Idle);
                    interval = DefaultInterval;
                    break;
                case StateStatus.Executing:
                    if (agent.CHP < agent.HP)
                    {
                        agent.RestoreHP(Math.Min(2, agent.HP-agent.CHP));
                    }
                    else
                    {
                        ChangeStatus(StateStatus.Completed);
                    }
                    break;
                case StateStatus.Completed:
                    agent.ChangeState(new EnemyIdle());
                    break;
                case StateStatus.Failed:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
