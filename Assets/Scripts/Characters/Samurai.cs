using UnityEngine;
// ReSharper disable InconsistentNaming
// ReSharper disable IdentifierTypo

namespace Characters
{
    public class Samurai : Player
    {
        private const int HP = 5;
        private const int MP = 6;
        private const int SPE = 10;
        private const int RAN = 8;
        private const int DMG = 10;
        private const int MDMG = 1;
        private const int DEF = 2;
        private const int MDEF = 6;
        private const int LVL = 1;
        private void Awake()
        {
            var charScript = GetComponent<Character>();
            charScript.SetupStats(HP, MP, SPE, RAN, DMG, DEF, MDMG, MDEF, LVL);
        }
    }
}