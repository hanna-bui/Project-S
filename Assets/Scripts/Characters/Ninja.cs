using UnityEngine;

namespace Characters
{
    public class Ninja : Character
    {

        // Start is called before the first frame update
        protected override void Start()
        {
            base.Start();

            HP = 6;
            CHP = HP;
            MP = 8;
            CMP = MP;
            SPE = 10;
            RAN = 10;
            DMG = 6;
            MDMG = 5;
            DEF = 1;
            MDEF = 2;
            LVL = 0;
            Sprite = GameObject.Find("Ninja");
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