using System;
using Characters;
using Characters.Enemies;
using JetBrains.Annotations;
using UnityEngine;

// ReSharper disable PossibleNullReferenceException

namespace Finite_State_Machine.Enemy_States
{
    public class EnemyHeal : State
    {

        private const float DefaultInterval = 1.6f;
        
        protected override void Initialize(Agent agent) 
        {
            agent.SetExactAnimation(0);
            interval = DefaultInterval;
            StateProgress();
        }

        protected override void Executing(Agent agent)
        {
            if (agent.NeedsHealing())
            {
                agent.RestoreHealth(2);
            }
            else
            {
                StateProgress();
            }
        }

        protected override void Completed(Agent agent)
        {
            agent.ChangeState(new PatternWalk());
        }
    }
}
