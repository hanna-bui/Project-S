using Characters;
using Characters.Enemies;

namespace Finite_State_Machine.States.Abilities
{
    public class ShurikenThrow : BasicAttack
    {
        private Enemy TargetStat { get; set; }

        private const float DefaultInterval = 1.2f;

        public ShurikenThrow()
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
            }
            else
            {
                StateProgress();
            }
        }
    }
}