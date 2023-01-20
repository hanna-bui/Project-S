using UnityEngine;

namespace Characters
{
    public class Knight : Character
    {

        // Start is called before the first frame update
        protected override void Start()
        {
            base.Start();

            HP = 10;
            CHP = HP;
            MP = 6;
            CMP = MP;
            SPE = 1;
            RAN = 5;
            DMG = 6;
            MDMG = 2;
            DEF = 10;
            MDEF = 8;
            LVL = 0;
            Sprite = GameObject.Find("Knight");
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