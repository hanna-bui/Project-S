using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chalice : Items
{
    // Start is called before the first frame update
    void Start()
    {
        ///temp values -> should restore just be a function to make halth = max?
        hpRestore = 10;
        manaRestore = 10;
    }
}
