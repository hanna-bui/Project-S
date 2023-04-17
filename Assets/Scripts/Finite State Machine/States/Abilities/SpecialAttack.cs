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
        
        protected override void Initialize(MoveableObject agent)
        {
            if (TargetStat is null)
            {
                TargetStat = agent.Target.GetComponent<Enemy>();
                agent.TargetLocation = TargetStat.Position();
                agent.CalculateDirection();
                interval = DefaultInterval;
            }
            ChangeStatus(StateStatus.Executing);
        }

        protected override void Executing(MoveableObject agent)
        {
            if (TargetStat is not null && TargetStat.isAttackable())
            {
                agent.SetAnimations(Motion.Attack);
                TargetStat.TakeDamage(agent.DMG);
                ChangeStatus(StateStatus.Completed);
            }
        }

        protected override void Completed(MoveableObject agent)
        {
            agent.ConfigState();
        }
    }
}