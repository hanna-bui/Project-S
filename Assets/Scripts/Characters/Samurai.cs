using UnityEngine;

namespace Characters
{
    public class Samurai : Character
    {
        
        // Start is called before the first frame update
        protected override void Start()
        {
            base.Start();
            
            hp = 5;
            chp = 5;
            mp = 6;
            cmp = 6;
            spe = 10;
            ran = 8;
            dmg = 10;
            mdmg = 1;
            def = 2;
            mdef = 6;
            lvl = 0;
            sprite = GameObject.Find("Samurai");
            
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