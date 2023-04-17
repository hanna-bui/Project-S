using System.Collections.Generic;
using Finite_State_Machine;
using Finite_State_Machine.States;
using Finite_State_Machine.States.Abilities;
using UnityEngine;

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
        /// Kunai Throw
        /// </summary>
        protected override State A1()
        {
            var newState = new KunaiThrow();
            ChangeState(newState);
            RestoreMana(-1);
            return newState;
        }

        /// <summary>
        /// Ability 2
        /// </summary>
        protected override State A2()
        {
            return null;
        }
        
        public override bool IsBasicAttack()
        {
            return base.IsBasicAttack() || CurrentState is ShurikenThrow;
        }

        public override bool isSpecialAttack()
        {
            return base.isSpecialAttack() || CurrentState is KunaiThrow;
        }

        protected override void LoadPlayer()
        {
            RestoreStats(isRespawning ? CurrentStats() : new List<int> { hp, mp, spe, ran, dmg, def, mdmg, mdef, lvl });
            base.LoadPlayer();
        }
    }
}