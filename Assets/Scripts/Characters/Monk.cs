using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monk : Character
{
    
    // Start is called before the first frame update
    void Start()
    {
        hp = 5;
        chp = 5;
        mp = 10;
        cmp = 10;
        spe = 6;
        ran = 6;
        dmg = 1;
        mdmg = 10;
        def = 2;
        mdef = 8;
        lvl = 0;
        sprite = GameObject.Find("Monk");
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
