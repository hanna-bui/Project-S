using Characters;
using Characters.Enemies;
using UnityEngine;
using Motion = Characters.Motion;

namespace Finite_State_Machine.States.Abilities
{
    public class SpecialAttack : State
    {
        private GameObject Target { get; set; }
        private Enemy TargetStat { get; set; }
        
        private const float DefaultInterval = 1.2f;
        
        protected SpecialAttack()
        {
            interval = 0f;
        }
        
        protected override void Initialize(Agent agent)
        {
            if (TargetStat is null)
            {
                TargetStat = agent.Target.GetComponent<Enemy>();
                agent.TargetLocation = TargetStat.Position();
                interval = DefaultInterval;
            }
            StateProgress();
        }

        protected override void Executing(Agent agent)
        {
            if (TargetStat is not null && TargetStat.CanAttack())
            {
                agent.SetAnimations(Motion.Attack);
                TargetStat.TakeDamage(agent.DMG);
                StateProgress();
            }
        }

        protected override void Completed(Agent agent)
        {
            agent.ConfigState();
        }
    }
}