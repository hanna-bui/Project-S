using UnityEngine;

namespace Characters
{
    public class Ninja : Character
    {

        // Start is called before the first frame update
        protected override void Start()
        {
            base.Start();

            hp = 6;
            chp = 6;
            mp = 8;
            cmp = 8;
            spe = 10;
            ran = 10;
            dmg = 6;
            mdmg = 5;
            def = 1;
            mdef = 2;
            lvl = 0;
            sprite = GameObject.Find("Ninja");
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