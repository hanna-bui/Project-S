using Characters;
using Characters.Enemy;
using Motion = Characters.Motion;

namespace Finite_State_Machine.States.Abilities
{
    public class KunaiThrow : Attack
    {
        public KunaiThrow()
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
            }
            agent.ChangeRange(1);
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