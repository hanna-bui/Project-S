using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bamboo : Character
{
    // Bamboo is an enemy character
    // Start is called before the first frame update
    void Start()
    {
        hp = 10;
        chp = 10;
        mp = 10;
        cmp = 10;
        spe = 1;
        ran = 2;
        dmg = 8;
        mdmg = 6;
        def = 6;
        mdef = 5;
        lvl = 0;
        ///icon = ;
        sprite = GameObject.Find("Bamboo");
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