using Characters;
using Characters.Enemies;
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

        protected override void Initialize(Agent agent)
        {
            if (TargetStat is not null) return;
            TargetStat = agent.Target.GetComponent<Enemy>();
            agent.TargetLocation = TargetStat.Position();
            interval = DefaultInterval;
        }

        protected override void Executing(Agent agent)
        {
            if (TargetStat is not null && TargetStat.CanAttack())
            {
                if (agent is Samurai) TargetStat.EquipFX(agent.fx2);
                agent.SetAnimations(Motion.Attack);
                TargetStat.TakeDamage(agent.DMG);
                agent.RestoreMana(-1);
            }
            else
            {
                StateProgress();
            }
        }

        protected override void Completed(Agent agent)
        {
            agent.ConfigState();
        }
    }
}