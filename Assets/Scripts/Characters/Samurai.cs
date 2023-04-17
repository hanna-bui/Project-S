using System.Collections.Generic;
using Finite_State_Machine;
using Finite_State_Machine.States.Abilities;

// ReSharper disable InconsistentNaming
// ReSharper disable IdentifierTypo

namespace Characters
{
    public class Samurai : Character
    {
        private const int hp = 5;
        private const int mp = 6;
        private const int spe = 10;
        private const int ran = 6;
        private const int dmg = 10;
        private const int def = 2;
        private const int mdmg = 1;
        private const int mdef = 6;
        private const int lvl = 1;
        
        /// <summary>
        /// Kunai Throw
        /// </summary>
        protected override State a1()
        {
            if (Target is null && !isBasicAttack()) return null;
            var newState = new DualWield();
            AddState(newState);
            RestoreMana(-1);
            return newState;
        }
        
        public override bool isBasicAttack()
        {
            return base.isBasicAttack();
        }

        public override bool isSpecialAttack()
        {
            return base.isSpecialAttack() || CurrentState is DualWield;
        }

        protected override void LoadPlayer()
        {
            RestoreStats(isRespawning ? CLS() : new List<int> { hp, mp, spe, ran, dmg, def, mdmg, mdef, lvl });
            base.LoadPlayer();
        }
    }
}