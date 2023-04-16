using System;
using Characters;
using UnityEngine;
using Action = Characters.Enemy.Action;

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
        
        protected override void Initialize(MoveableObject agent) 
        {
            if (TargetStat is null)
            {
                TargetStat = Target.GetComponent<Character>();
                            
                agent.TargetLocation = TargetStat.Position();
                agent.CalculateDirection();

                interval = DefaultInterval;
                ChangeStatus(StateStatus.Executing);
            }
        }

        protected override void Executing(MoveableObject agent)
        {
            if (TargetStat is not null && TargetStat.isAttackable())
            {
                agent.SetAnimations(Action.Attack);
                TargetStat.TakeDamage(agent.DMG);
            }
            else
            {
                ChangeStatus(StateStatus.Completed);
                interval = 0;
            }
        }

        protected override void Completed(MoveableObject agent)
        {
            if (agent.NeedsHealing())
                agent.ChangeState(new EnemyHeal());
            else
                agent.ChangeState(new PatternWalk());
        }
    }
}