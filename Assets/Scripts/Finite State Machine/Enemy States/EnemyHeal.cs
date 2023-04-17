using System;
using Characters;
using Characters.Enemy;
using JetBrains.Annotations;
using UnityEngine;
using Action = Characters.Enemies.Action;
// ReSharper disable PossibleNullReferenceException

namespace Finite_State_Machine.Enemy_States
{
    public class EnemyHeal : State
    {

        private const float DefaultInterval = 1.6f;
        
        protected override void Initialize(MoveableObject agent) 
        {
            agent.SetAnimations(Action.Idle);
            interval = DefaultInterval;
            StateProgress();
        }

        protected override void Executing(MoveableObject agent)
        {
            if (agent.NeedsHealing())
            {
                agent.RestoreHP(2);
            }
            else
            {
                StateProgress();
            }
        }

        protected override void Completed(MoveableObject agent)
        {
            agent.ChangeState(new PatternWalk());
        }
    }
}
