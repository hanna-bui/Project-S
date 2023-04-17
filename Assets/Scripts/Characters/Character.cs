using System.Diagnostics.CodeAnalysis;
using Finite_State_Machine;
using Finite_State_Machine.States;
using Finite_State_Machine.States.Abilities;
using Managers;
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
        public const int Special1 = 12;
        public const int Special2 = 13;
    }
    public class Character : MoveableObject
    {
        // All Characters have:
        
        
        private TextMeshProUGUI mpValue;

        // Assets
        protected GameObject weapon { get; set; } // Main Weapon Sprite
        protected GameObject wa1 { get; set; } // A1 Weapon Sprite
        protected GameObject wa2 { get; set; } // A2 Weapon Sprite
        protected GameObject wa3 { get; set; } // A3 Weapon Sprite
        
        private SelectUI select;

        private void OnTriggerEnter2D(Collider2D col)
        {
            if (col.gameObject.CompareTag("Item") && CurrentState is not WalkToLocation)
            {
                ChangeState(new ItemPickup(col.GetComponent<Item>()));
            }
            else if (col.gameObject.name.Contains("Inactive"))
            {
                Debug.Log("Level Complete!");
                GameManager.instance.Next();
            }
            else if (col.gameObject.name.Contains("Active"))
            {
                Debug.Log("You Won!");
                GameManager.instance.Win();
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

        protected override void Initializing()
        {
            parentName = "Characters";
            
            States.Push(new PlayerIdle());

            mpValue = transform.GetChild(1).GetChild(1).GetComponent<TextMeshProUGUI>();

            LoadPlayer();
            
            radius = 0.5f;
        }

        protected virtual void LoadPlayer()
        {
            UpdateUI();
        }

        protected override void Update()
        {
            CurrentState = GetTop();
            CurrentState.Execute(this, Time.deltaTime);

            UpdateUI();
            inputToState();
            
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

        public override void UpdateUI()
        {
            hpValue.text = "HP: " + CHP + "";
            mpValue.text = "MP: " + CMP + "";
        }
        
        #region Stats Management

        public override void ChangeRange(int rangeValue)
        {
            base.ChangeRange(rangeValue);
            child_collider.radius = RAN*5;
        }

        [SuppressMessage("ReSharper", "Unity.PerformanceCriticalCodeInvocation")]
        public override void TakeDamage(int dmg)
        {
            base.TakeDamage(dmg);
            if (CHP != 0) return;
            Debug.Log("You lost...");
            GameManager.instance.Lose();
        }

        #endregion
        
        #region States

        public override bool ConfigState()
        {
            if (States.Count > 1)
            {
                States.Pop();
                if (!hasSpecialAttack()) return true;
                States.Pop();
                States.Push(new PlayerIdle());
                return false;
            }
            States.Pop();
            States.Push(new PlayerIdle());
            return false;
        }

        private bool hasSpecialAttack()
        {
            return States.Count > 1 && isBasicAttack();
        }

        public override bool isBasicAttack()
        {
            return CurrentState is BasicAttack;
        }

        public virtual bool isSpecialAttack()
        {
            return CurrentState is SpecialAttack;
        }

        [SuppressMessage("ReSharper", "Unity.PerformanceCriticalCodeInvocation")]
        private void inputToState()
        {
            if (Input.GetMouseButtonDown(0))
            {
                TargetLocation = camera.ScreenToWorldPoint(Input.mousePosition);
                
                var origin = new Vector2(TargetLocation.x, TargetLocation.y);
                var t = Physics2D.Raycast(origin, Vector2.zero, 0f);
                
                if (t && t.transform.CompareTag("Enemy"))
                {
                    select = t.transform.GetComponent<Enemies.Enemy>().select;
                    if (isBasicAttack())
                    {
                        if (CurrentState.CurrentStatus is StateStatus.Initialize)
                        {
                            CurrentState.ChangeStatus(StateStatus.Executing);
                        } 
                    }
                    switch (select.isOn)
                    {
                        case true:
                        {
                            if (isSpecialAttack()) AddState(new WalkToLocation());
                            else if (!isBasicAttack()) ChangeState(new WalkToLocation());
                            break;
                        }
                        case false:
                            select.Toggle();
                            break;
                    }
                }
                else
                {
                    if (isSpecialAttack()) AddState(new WalkToLocation());
                    else ChangeState(new WalkToLocation());
                }
            }
            
            else if (Input.GetKeyDown(KeyCode.F) && CurrentState is ItemPickup)
                CurrentState.ChangeStatus(StateStatus.Executing);
            
            else if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                a1();
            }
            
            else if (Input.GetKeyDown(KeyCode.Escape))
            {
                if(select!=null)
                {
                    select.Toggle();
                    if (isBasicAttack()) CurrentState.ChangeStatus(StateStatus.Completed);
                }
            }
        }

        #endregion
    }
}