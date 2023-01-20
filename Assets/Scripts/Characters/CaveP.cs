using UnityEngine;

namespace Characters
{

    public class CaveP : Character
    {

        // Start is called before the first frame update
        protected override void Start()
        {
            base.Start();

            HP = 10;
            CHP = HP;
            MP = 5;
            CMP =  MP;
            SPE = 8;
            RAN = 6;
            DMG = 10;
            MDMG = 1;
            DEF = 6;
            MDEF = 2;
            LVL = 0;
            Sprite = GameObject.Find("CaveP");
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