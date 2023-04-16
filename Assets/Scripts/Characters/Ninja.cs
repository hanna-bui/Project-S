using System.Collections.Generic;
using Finite_State_Machine;
using Finite_State_Machine.States.Abilities;

// ReSharper disable InconsistentNaming
// ReSharper disable IdentifierTypo

namespace Characters
{
    public class Ninja : Character
    {
        private const int hp = 6;
        private const int mp = 8;
        private const int spe = 10;
        private const int ran = 10;
        private const int dmg = 6;
        private const int def = 1;
        private const int mdmg = 5;
        private const int mdef = 2;
        private const int lvl = 1;
        
        /// <summary>
        /// Ability 1
        /// </summary>
        protected override State a1()
        {
            return new KunaiThrow();
        }

        /// <summary>
        /// Ability 2
        /// </summary>
        protected override State a2()
        {
            return null;
        }
        
        public override void LoadPlayer()
        {
            RestoreStats(isRespawning ? CLS() : new List<int> { hp, mp, spe, ran, dmg, def, mdmg, mdef, lvl });
            base.LoadPlayer();
        }
    }
}