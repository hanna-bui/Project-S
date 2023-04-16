using System;
using Characters.Enemy;
using Characters;
using UnityEngine;
using Motion = Characters.Motion;

// ReSharper disable PossibleNullReferenceException

namespace Finite_State_Machine.States
{

    public class Attack : State
    {
        private GameObject Target { get; set; }
        private Enemy TargetStat { get; set; }

        private const float DefaultInterval = 1.2f;

        public Attack(GameObject target)
        {
            interval = 0f;
            Target = target;
        }

        // public override void Execute(MoveableObject agent)
        // {
        //     
        // }

        protected override void Initialize(MoveableObject agent)
        {
            if (TargetStat is null)
            {
                TargetStat = Target.GetComponent<Enemy>();
                agent.TargetLocation = TargetStat.Position();
                agent.CalculateDirection();
                interval = DefaultInterval;
            }
        }

        protected override void Executing(MoveableObject agent)
        {
            if (TargetStat is not null && TargetStat.isAttackable())
            {
                agent.SetAnimations(Motion.Attack);
                TargetStat.TakeDamage(agent.DMG);
            }
            else
            {
                ChangeStatus(StateStatus.Completed);
            }
        }

        protected override void Completed(MoveableObject agent)
        {
            agent.ChangeState(new PlayerIdle());
        }
    }
}