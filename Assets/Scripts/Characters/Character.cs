using Finite_State_Machine;
using Finite_State_Machine.States;
using UnityEngine;
using Mirror;
using Item = Items.Items;

// using Movement.Steering_Behaviour;
// ReSharper disable InconsistentNaming
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable Unity.UnknownTag

namespace Characters 
{
    /// <summary>
    /// to control multiple characters, this class needs to be a networkBehaviour
    public class Character : MoveableObject
    {
        public GameObject prefab;
        // All Characters have:
        

        // Assets
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
        private void OnTriggerStay2D(Collider2D col)
        {
            if (col.gameObject.CompareTag("Item") && currentState is not WalkToLocation)
            {
                ChangeState(new ItemPickup(col.GetComponent<Item>()));
            }
        }
        private void OnTriggerExit2D(Collider2D col)
        {
            if (col.gameObject.CompareTag("Item"))
            {
                ChangeState(new PlayerIdle());
            }
        }
        
        protected override void Start()
        {
            base.Start();
            currentState = new PlayerIdle();
            radius = 0.5f;
        }

        protected override void Update()
        {
 
            currentState.Execute(this, Time.deltaTime);
            
            if (Input.GetMouseButtonDown(0))
            {
                if (!isLocalPlayer)
                {
                    return;
                }
                TargetLocation = camera.ScreenToWorldPoint(Input.mousePosition);
                
                
                var origin = new Vector2(TargetLocation.x, TargetLocation.y);
                var t = Physics2D.Raycast(origin, Vector2.zero, 0f);
                
                if (t && t.transform.CompareTag("Enemy"))
                {
                    if (currentState is Attack)
                    {
                        if (currentState.CurrentStatus is StateStatus.Initialize)
                        {
                            currentState.ChangeStatus(StateStatus.Executing);
                        }
                        else
                        {
                            
                        }
                    }
                    else
                    {
                        var newState = new Attack(this, t.transform.gameObject);
                        ChangeState(newState);
                    }
                }
                else
                {
                    ChangeState(new WalkToLocation());
                }
            }

            if (Input.GetKeyDown(KeyCode.F) && currentState is ItemPickup)
            {
                currentState.ChangeStatus(StateStatus.Executing);
            }
        }


        private void Move()
        {

            currentState.Execute(this, Time.deltaTime);

            if (Input.GetMouseButtonDown(0))
            {
                TargetLocation = camera.ScreenToWorldPoint(Input.mousePosition);


                var origin = new Vector2(TargetLocation.x, TargetLocation.y);
                var t = Physics2D.Raycast(origin, Vector2.zero, 0f);

                if (t && t.transform.CompareTag("Enemy"))
                {
                    if (currentState is Attack)
                    {
                        if (currentState.CurrentStatus is StateStatus.Initialize)
                        {
                            currentState.ChangeStatus(StateStatus.Executing);
                        }
                        else
                        {

                        }
                    }
                    else
                    {
                        var newState = new Attack(this, t.transform.gameObject);
                        ChangeState(newState);
                    }
                }
                else
                {
                    ChangeState(new WalkToLocation());
                }
            }

            if (Input.GetKeyDown(KeyCode.F) && currentState is ItemPickup)
            {
                currentState.ChangeStatus(StateStatus.Executing);
            }
        }

    }
}