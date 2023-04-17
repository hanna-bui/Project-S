using Characters;
using Motion = Characters.Motion;

namespace Finite_State_Machine.States.Abilities
{
    public class KunaiThrow : SpecialAttack
    {
        public KunaiThrow()
        {
            interval = 0f;
            timeLimit = 10f;
        }
        
        protected override void Initialize(MoveableObject agent)
        {
            agent.ChangeRange(1);
            agent.SetFacing(1);
            agent.SetExactAnimation(Motion.Special1);
        }

        protected override void Executing(MoveableObject agent)
        {
        }

        protected override void Completed(MoveableObject agent)
        {
            agent.ChangeDamage(-1);
            agent.ConfigState();
        }
    }
}