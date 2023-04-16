using System;
using Characters;
using Characters.Enemy;
using JetBrains.Annotations;
using UnityEngine;
using Action = Characters.Enemy.Action;
// ReSharper disable PossibleNullReferenceException

namespace Finite_State_Machine.Enemy_States
{
    public class EnemyHeal : State
    {

        private const float DefaultInterval = 1.6f;
        
        public override void Execute(MoveableObject agent)
        {
            // agent = agent as Enemy;
            switch (CurrentStatus)
            {
                case StateStatus.Initialize:
                    agent.SetAnimations(Action.Charge);
                    interval = DefaultInterval;
                    ChangeStatus(StateStatus.Executing);
                    break;
                case StateStatus.Executing:
                    if (agent.NeedsHealing())
                    {
                        agent.RestoreHP(2);
                    }
                    else
                    {
                        ChangeStatus(StateStatus.Completed);
                    }
                    break;
                case StateStatus.Completed:
                    agent.ChangeState(new PatternWalk());
                    break;
                case StateStatus.Failed:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
