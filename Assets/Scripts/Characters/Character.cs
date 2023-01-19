using Finite_State_Machine.States;
using UnityEngine;
// using Movement.Steering_Behaviour;
// ReSharper disable InconsistentNaming
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

namespace Characters
{

    public class Character : MoveableObject
    {
        // All Characters have:
        protected float hp { get; set; } // health points
        protected float chp { get; set; } // current health points
        protected float mp { get; set; } // mana points
        protected float cmp { get; set; } // current mana points
        public float spe { get; set; } // speed
        protected float ran { get; set; } // range
        protected float dmg { get; set; } // damage
        
        protected float mdmg { get; set; } // magic damage
        protected float def { get; set; } // defense
        protected float mdef { get; set; } // magic defense

        protected int lvl { get; set; } // level

        // Assets
        protected GameObject sprite { get; set; } // Character Sprite
        protected GameObject weapon { get; set; } // Main Weapon Sprite
        protected GameObject wa1 { get; set; } // A1 Weapon Sprite
        protected GameObject wa2 { get; set; } // A2 Weapon Sprite

        protected GameObject wa3 { get; set; } // A3 Weapon Sprite

        // In enemies, unused abilities can be set to have no effect
        protected void a1()
        {
        } // ability 1

        protected void a2()
        {
        } // ability 2

        protected void a3()
        {
        } // ability 3

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
        protected override void Start()
        {
            base.Start();
            currentState = new PlayerIdle();
        }
        
        protected override void Update()
        {
            base.Update();
            
            if (Input.GetMouseButtonDown(0))
            {
                TargetLocation = camera.ScreenToWorldPoint(Input.mousePosition);
                
                
                var origin = new Vector2(TargetLocation.x, TargetLocation.y);
                var t = Physics2D.Raycast(origin, Vector2.zero, 0f);
                
                if (t)
                {
                    ChangeState(new Attack());
                    print(t.transform.gameObject.tag);
                }
                else
                {
                    ChangeState(new WalkToLocation());
                }
            }
        }
        
        protected void SetupCollider()
        {
            var rig = gameObject.AddComponent<Rigidbody2D>();
            rig.bodyType = RigidbodyType2D.Kinematic;
            rig.simulated = true;
            
            var circle = gameObject.AddComponent<CircleCollider2D>();
            circle.radius = ran/5;
            circle.isTrigger = true;
            
            Debug.Log(spe);
        }

    }
}