using Characters;
using UnityEngine;
using Motion = Characters.Motion;

namespace Finite_State_Machine.States.Abilities
{
    public class KunaiThrow : SpecialAttack
    {
        
        public KunaiThrow()
        {
            timeLimit = 10f;
            isABuff = true;
        }
        
        protected override void Initialize(MoveableObject agent)
        {
            agent.ChangeRange(1);
            agent.SetExactAnimation(Motion.Special1);
            agent.SetFacing(0);
            interval = 1f;
            StateProgress();
        }

        protected override void Executing(MoveableObject agent)
        {
            
        }

        protected override void Completed(MoveableObject agent)
        {
            agent.ChangeRange(-1);
            agent.RemoveSpecial1(this);
            Debug.Log("Completed Kunai Throw");
            // agent.ConfigState();
        }
    }
}