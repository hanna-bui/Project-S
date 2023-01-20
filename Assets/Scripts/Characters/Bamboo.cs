using UnityEngine;

namespace Characters
{

    public class Bamboo : Character
    {
        // Bamboo is an enemy character
        // Start is called before the first frame update
        protected override void Start()
        {
            base.Start();

            HP = 10;
            CHP = HP;
            MP = 10;
            CMP = MP;
            SPE = 1;
            RAN = 2;
            DMG = 8;
            MDMG = 6;
            DEF = 6;
            MDEF = 5;
            LVL = 0;
            ///icon = ;
            Sprite = GameObject.Find("Bamboo");
            //weapon = ;
            //wa1 = ;
            //wa2 = ;
            //wa3 = ;
            SetupCollider();
        }

        // Update is called once per frame
        protected override void Update()
        {
            base.Update();
        }
    }
}