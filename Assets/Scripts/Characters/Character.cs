using System;
using System.Collections.Generic;
using Finite_State_Machine;
using Finite_State_Machine.States;
using UnityEngine;
using Item = Items.Items;

// using Movement.Steering_Behaviour;
// ReSharper disable InconsistentNaming
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable Unity.UnknownTag

namespace Characters
{
    public static class Direction
    {
        public const int Down = 0;
        public const int Left = 1;
        public const int Right = 2;
        public const int Up = 3;
    }
    
    public static class Motion
    {
        public const int Attack = 0;
        public const int Idle = 1;
        public const int Walk = 2;
    }
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
        
        private void OnTriggerEnter2D(Collider2D col)
        {
            if (col.gameObject.CompareTag("Item") && currentState is not WalkToLocation)
            {
                ChangeState(new ItemPickup(col.GetComponent<Item>()));
            }
        }
        private void OnTriggerStay2D(Collider2D col)
        {
            if (col.gameObject.CompareTag("Item") && currentState is not WalkToLocation)
            {
                ChangeState(new ItemPickup(col.GetComponent<Item>()));
            }
            if (col.gameObject.CompareTag("Enemy") && currentState is not WalkToLocation)
            {
                Debug.Log("Colliding with Enemy");
                var newState = new Attack(this, col.transform.gameObject);
                ChangeState(newState);
            }
        }
        private void OnTriggerExit2D(Collider2D col)
        {
            if (col.gameObject.CompareTag("Item") && currentState is ItemPickup)
            {
                ChangeState(new PlayerIdle());
            }
        }
        
        protected override void Start()
        {
            base.Start();
            States = new Stack<State>();
            States.Push(new PlayerIdle());
            radius = 0.5f;
        }

        protected override void Update()
        {
            currentState = GetTop();
            currentState.Execute(this, Time.deltaTime);
            
            if (Input.GetMouseButtonDown(0))
            {
                TargetLocation = camera.ScreenToWorldPoint(Input.mousePosition);
                
                var origin = new Vector2(TargetLocation.x, TargetLocation.y);
                var t = Physics2D.Raycast(origin, Vector2.zero, 0f);
                
                if (t && t.transform.CompareTag("Enemy"))
                {
                    Debug.Log("Clicking Enemy");
                    if (currentState is Attack)
                    {
                        SetAnimations(Motion.Attack);
                        if (currentState.CurrentStatus is StateStatus.Initialize)
                        {
                            currentState.ChangeStatus(StateStatus.Executing);
                        } 
                    }
                    else
                        ChangeState(new WalkToLocation());
                }
                else
                    ChangeState(new WalkToLocation());
            }

            if (Input.GetKeyDown(KeyCode.F) && currentState is ItemPickup)
                currentState.ChangeStatus(StateStatus.Executing);
        }
        
        public override void CalculateDirection()
        {
            var position = transform.position;
            
            var p1 = new Vector2(position.x, position.y);
            var p2 = new Vector2(TargetLocation.x, TargetLocation.y);

            var angleFloat = Mathf.Atan2(p2.y - p1.y, p2.x - p1.x) * 180 / Mathf.PI;
            var angleInt = Mathf.CeilToInt(angleFloat);

            facing = angleInt switch
            {
                <= 135 and >= 45 => Direction.Up,
                < 45 and > -45 => Direction.Right,
                <= -45 and >= -135 => Direction.Down,
                _ => Direction.Left
            };
        }
        
        public override void StopAnimation()
        {
            SetAnimations(Motion.Idle);
        }
        
        public override void SetAnimations(int motion)
        {
            var index = facing;
            animator.Play(animations[index + (4 * motion)].name);
        }

        public override bool IsSubState()
        {
            if (States.Count > 1)
            {
                States.Pop();
                return true;
            }
            States.Pop();
            States.Push(new PlayerIdle());
            return false;
        }
    }
}