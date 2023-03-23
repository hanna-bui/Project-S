using UnityEngine;
// ReSharper disable InconsistentNaming
// ReSharper disable IdentifierTypo

namespace Characters
{
    public class Samurai : Character
    {
        private const float HP = 5;
        private const float MP = 6;
        private const float SPE = 10;
        private const float RAN = 8;
        private const float DMG = 10;
        private const float MDMG = 1;
        private const float DEF = 2;
        private const float MDEF = 6;
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