using UnityEngine;

namespace Characters
{
    public class Monk : Character
    {
        
        // Start is called before the first frame update

        
        protected override void Start()
        {
            base.Start();
            
            HP = 5;
            CHP = HP;
            MP = 10;
            CMP = MP;
            SPE = 6;
            RAN = 6;
            DMG = 1;
            MDMG = 10;
            DEF = 2;
            MDEF = 8;
            LVL = 0;
            Sprite = GameObject.Find("Monk");
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