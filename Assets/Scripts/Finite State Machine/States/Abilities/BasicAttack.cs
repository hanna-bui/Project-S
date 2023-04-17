using Characters;
using Characters.Enemies;
using Characters.Enemy;
using UnityEngine;
using Motion = Characters.Motion;

// ReSharper disable PossibleNullReferenceException

namespace Finite_State_Machine.States.Abilities
{

    public class BasicAttack : State
    {
        private Enemy TargetStat { get; set; }

        private const float DefaultInterval = 1.2f;

        public BasicAttack()
        {
            interval = 0f;
        }

        protected override void Initialize(MoveableObject agent)
        {
            if (TargetStat is not null) return;
            TargetStat = agent.Target.GetComponent<Enemy>();
            agent.TargetLocation = TargetStat.Position();
            agent.CalculateDirection();
            interval = DefaultInterval;
        }

        protected override void Executing(MoveableObject agent)
        {
            if (TargetStat is not null && TargetStat.isAttackable())
            {
                agent.SetAnimations(Motion.Attack);
                TargetStat.TakeDamage(agent.DMG);
                agent.RestoreMana(-1);
            }
            else
            {
                StateProgress();
            }
        }

        protected override void Completed(MoveableObject agent)
        {
            agent.ConfigState();
        }
    }
}