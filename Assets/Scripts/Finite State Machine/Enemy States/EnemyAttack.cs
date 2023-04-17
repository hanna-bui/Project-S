using System;
using Characters;
using Characters.Enemies;
using UnityEngine;

// ReSharper disable PossibleNullReferenceException

namespace Finite_State_Machine.Enemy_States
{
    public class EnemyAttack : State
    {
        private GameObject Target { get; set; }
        private Character TargetStat { get; set; }

        private const float DefaultInterval = 1.6f;

        public EnemyAttack(GameObject target)
        {
            interval = 0f;
            Target = target;
        }
        
        protected override void Initialize(Agent agent) 
        {
            if (TargetStat is null)
            {
                TargetStat = Target.GetComponent<Character>();
                            
                agent.TargetLocation = TargetStat.Position();
                if (agent is not Boss) agent.SetExactAnimation(0);

                interval = DefaultInterval;
                StateProgress();
            }
        }

        protected override void Executing(Agent agent)
        {
            if (TargetStat is not null && TargetStat.CanAttack())
            {
                if (agent is Boss) agent.SetAnimations(BossAction.ATTACK);

                TargetStat.TakeDamage(agent.DMG);
            }
            else
            {
                StateProgress();
                interval = 0;
            }
        }

        protected override void Completed(Agent agent)
        {
            agent.ConfigState();
        }
    }
}