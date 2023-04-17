using Characters;
using Characters.Enemies;
using Motion = Characters.Motion;

namespace Finite_State_Machine.States.Abilities
{
    public class DualWield : SpecialAttack
    {
        private Enemy TargetStat { get; set; }

        private const float DefaultInterval = 0f;

        private static float _multiplier = 0.25f;

        public DualWield()
        {
            interval = 0f;
        }

        protected override void Initialize(Agent agent)
        {
            interval = DefaultInterval;
            if (TargetStat is null && agent.Target is not null)
            {
                _multiplier = (float)(agent.DMG * 0.25);
                agent.ChangeDamage((int)(_multiplier));
                agent.SetExactAnimation(Motion.Special1);
                agent.SetFacing(0);
                TargetStat = agent.Target.GetComponent<Enemy>();
                agent.TargetLocation = TargetStat.Position();
            }

            StateProgress();
        }

        protected override void Executing(Agent agent)
        {
            if (TargetStat is not null && TargetStat.CanAttack())
            {
                TargetStat.EquipFX(agent.fx1);
                TargetStat.TakeDamage(agent.DMG);
            }
            StateProgress();
        }


        protected override void Completed(Agent agent)
        {
            agent.ChangeRange((int)(-_multiplier));
            agent.RemoveSpecial1(this);
            // agent.ConfigState();
        }
    }
}