using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ninja : Character
{
    
    // Start is called before the first frame update
    void Start()
    {
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
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("Movement2");
        }
    }
}
