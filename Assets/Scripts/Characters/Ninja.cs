using UnityEngine;
// ReSharper disable InconsistentNaming
// ReSharper disable IdentifierTypo

namespace Characters
{
    public class Ninja : Player
    {
        private const int HP = 6;
        private const int MP = 8;
        private const int SPE = 10;
        private const int RAN = 10;
        private const int DMG = 6;
        private const int MDMG = 5;
        private const int DEF = 1;
        private const int MDEF = 2;
        private const int LVL = 1;

        private void Awake()
        {
            var charScript = GetComponent<Character>();
            charScript.SetupStats(HP, MP, SPE, RAN, DMG, DEF, MDMG, MDEF, LVL);
        }
    }
}