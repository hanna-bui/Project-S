using UnityEngine;
// ReSharper disable InconsistentNaming
// ReSharper disable IdentifierTypo

namespace Characters
{
    public class Monk : Player
    {
        private const int HP = 5;
        private const int MP = 10;
        private const int SPE = 6;
        private const int RAN = 6;
        private const int DMG = 1;
        private const int MDMG = 10;
        private const int DEF = 2;
        private const int MDEF = 8;
        private const int LVL = 1;
        private void Awake()
        {
            var charScript = GetComponent<Character>();
            charScript.SetupStats(HP, MP, SPE, RAN, DMG, DEF, MDMG, MDEF, LVL);
        }
    }
}