using UnityEngine;
// ReSharper disable InconsistentNaming
// ReSharper disable IdentifierTypo

namespace Characters
{
    public class Knight : MonoBehaviour
    {
        private const float HP = 10;
        private const float MP = 6;
        private const float SPE = 1;
        private const float RAN = 5;
        private const float DMG = 6;
        private const float MDMG = 2;
        private const float DEF = 10;
        private const float MDEF = 8;
        private const int LVL = 1;
        
        private void Awake()
        {
            var charScript = GetComponent<Character>();
            charScript.SetupStats(HP, MP, SPE, RAN, DMG, DEF, MDMG, MDEF, LVL);
        }
    }
}