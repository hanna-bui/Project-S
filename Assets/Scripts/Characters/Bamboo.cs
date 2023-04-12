using UnityEngine;
// ReSharper disable InconsistentNaming
// ReSharper disable IdentifierTypo

namespace Characters
{
    public class Bamboo : MonoBehaviour
    {
        private const float HP = 10;
        private const float MP = 10;
        private const float SPE = 1;
        private const float RAN = 2;
        private const float DMG = 8;
        private const float MDMG = 6;
        private const float DEF = 6;
        private const float MDEF = 5;
        private const int LVL = 1;
        
        private void Awake()
        {
            var charScript = GetComponent<Character>();
            charScript.SetupStats(HP, MP, SPE, RAN, DMG, DEF, MDMG, MDEF, LVL);
        }
    }
}