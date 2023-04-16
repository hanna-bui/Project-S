using System.Collections.Generic;
// ReSharper disable InconsistentNaming
// ReSharper disable IdentifierTypo

namespace Characters
{
    public class Monk : Character
    {
        private const int hp = 5;
        private const int mp = 10;
        private const int spe = 6;
        private const int ran = 6;
        private const int dmg = 1;
        private const int def = 2;
        private const int mdmg = 10;
        private const int mdef = 8;
        private const int lvl = 1;
        
        public override void LoadPlayer()
        {
            RestoreStats(isRespawning ? CLS() : new List<int> { hp, mp, spe, ran, dmg, def, mdmg, mdef, lvl });
            base.LoadPlayer();
        }
    }
}