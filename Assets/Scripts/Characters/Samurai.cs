using UnityEngine;

namespace Characters
{
    public class Samurai : Character
    {
        
        // Start is called before the first frame update
        protected override void Start()
        {
            base.Start();
            
            HP = 5;
            CHP = HP;
            MP = 6;
            CMP = MP;
            SPE = 10;
            RAN = 8;
            DMG = 10;
            MDMG = 1;
            DEF = 2;
            MDEF = 6;
            LVL = 0;
            Sprite = GameObject.Find("Samurai");
            
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