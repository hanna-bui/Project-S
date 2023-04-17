using System.Collections.Generic;
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

        protected override void LoadPlayer()
        {
            RestoreStats(isRespawning ? CLS() : new List<int> { hp, mp, spe, ran, dmg, def, mdmg, mdef, lvl });
            base.LoadPlayer();
        }
    }
}