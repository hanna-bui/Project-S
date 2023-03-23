using System.Collections.Generic;
using Finite_State_Machine;
using Finite_State_Machine.States;
using TMPro;
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
        public const int Hit = 3;
    }
    public class Character : MoveableObject
    {
        // All Characters have:
        
        private TextMeshProUGUI hpValue;
        private TextMeshProUGUI mpValue;

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
        
        private void OnTriggerEnter2D(Collider2D col)
        {
            if (col.gameObject.CompareTag("Item") && CurrentState is not WalkToLocation)
            {
                ChangeState(new ItemPickup(col.GetComponent<Item>()));
            }
        }
        private void OnTriggerStay2D(Collider2D col)
        {
            if (col.gameObject.CompareTag("Item") && CurrentState is not WalkToLocation)
            {
                ChangeState(new ItemPickup(col.GetComponent<Item>()));
            }
        }
        private void OnTriggerExit2D(Collider2D col)
        {
            if (col.gameObject.CompareTag("Item") && CurrentState is ItemPickup)
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

            SetupCollider();
            var child = transform.GetChild(0).gameObject.GetComponent<CharacterCollider>();
            child.SetupCollider(RAN);
            
            hpValue = transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>();
            hpValue.text = "HP: " + CHP + "";
            
            mpValue = transform.GetChild(1).GetChild(1).GetComponent<TextMeshProUGUI>();
            mpValue.text = "MP: " + CMP + "";
            
            var parent = GameObject.Find("Characters").transform;
            transform.parent = parent;
            transform.localScale = new Vector3(1, 1, 0);
        }

        protected override void Update()
        {
            if (!isLocalPlayer) return;
            
            CurrentState = GetTop();
            CurrentState.Execute(this, Time.deltaTime);
            
            if (Input.GetMouseButtonDown(0))
            {
                TargetLocation = camera.ScreenToWorldPoint(Input.mousePosition);
                
                var origin = new Vector2(TargetLocation.x, TargetLocation.y);
                var t = Physics2D.Raycast(origin, Vector2.zero, 0f);
                
                if (t && t.transform.CompareTag("Enemy"))
                {
                    if (CurrentState is Attack)
                    {
                        if (CurrentState.CurrentStatus is StateStatus.Initialize)
                        {
                            CurrentState.ChangeStatus(StateStatus.Executing);
                        } 
                    }
                    else
                        ChangeState(new WalkToLocation());
                }
                else
                    ChangeState(new WalkToLocation());
            }

            if (Input.GetKeyDown(KeyCode.F) && CurrentState is ItemPickup)
                CurrentState.ChangeStatus(StateStatus.Executing);
        }

        #region Animations

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
            var animStateInfo = animator.GetCurrentAnimatorStateInfo(0);
            var NTime = animStateInfo.normalizedTime;
            if (NTime > 0)
                animator.Play(animations[index + (4 * motion)].name);
        }

        #endregion

        #region States

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

        #endregion

        
        
        public override void TakeDamage(float dmg)
        {
            CHP -= dmg;
            hpValue.text = "HP: " + CHP + "";
        }
    }
}