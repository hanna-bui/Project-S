using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    // All Characters have:
    protected float hp { get; set; } // health points
    protected float mp { get; set; } // mana points
    protected float spe { get; set; } // speed
    protected float ran { get; set; } // range
    protected float dmg { get; set; } // damage
    protected float mdmg { get; set; } // magic damage
    protected float def { get; set; } // defense
    protected float mdef { get; set; } // magic defense
    // Do Enemies have abilities? If not we can:
    // - Move to Player class, OR
    // - Just set these to null in enemies :)
    protected Ability a1 { get; set; } // ability 1
    protected Ability a2 { get; set; } // ability 2
    protected Ability a3 { get; set; } // ability 3
    
    public Character() {}

    /* Do we need methods like this?
     How are we going to deal damage, heal, etc?
     public float gethp()
    {
        return hp;
    }
    // ALL characters take damage
    // Characters can only be damaged up to hp=0
    public void takeDamage(float dmg)
    {
        hp -= dmg;
        if (hp < 0) hp = 0;
    }

    public void inchp(int inc)
    {
        hp += inc;
    }*/
}