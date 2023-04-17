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
                agent.SetAnimations(Motion.Attack);
                agent.RestoreMana(-1);
            }
            else
            {
                StateProgress();
            }
        }
    }
}