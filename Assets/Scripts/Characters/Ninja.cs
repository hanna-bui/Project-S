using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ninja : Character
{
    
    // Start is called before the first frame update
    void Start()
    {
        sprite = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
