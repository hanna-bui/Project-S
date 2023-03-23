using UnityEngine;
// ReSharper disable InconsistentNaming
// ReSharper disable IdentifierTypo

namespace Characters
{
    public class Ninja : Character
    {
        private const float HP = 6;
        private const float MP = 8;
        private const float SPE = 10;
        private const float RAN = 10;
        private const float DMG = 6;
        private const float MDMG = 5;
        private const float DEF = 1;
        private const float MDEF = 2;
        private const int LVL = 1;

        private void Awake()
        {
            var charScript = GetComponent<Character>();
            charScript.SetupStats(HP, MP, SPE, RAN, DMG, DEF, MDMG, MDEF, LVL);
        }
        protected override void Start()
        {
        }

        protected override void Update()
        {
        }
    }
}