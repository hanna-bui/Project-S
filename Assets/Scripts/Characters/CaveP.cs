using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaveP : Character
{
    
    // Start is called before the first frame update
    void Start()
    {
        hp = 10;
        chp = 10;
        mp = 5;
        cmp = 5;
        spe = 8;
        ran = 6;
        dmg = 10;
        mdmg = 1;
        def = 6;
        mdef = 2;
        lvl = 0;
        sprite = GameObject.Find("CaveP");
        //weapon = ;
        //wa1 = ;
        //wa2 = ;
        //wa3 = ;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
