using System;
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
    public class Character : Agent
    {
        // All Characters have:
        
        
        private TextMeshProUGUI mpValue;

        // Assets
        
        protected GameObject wa2 { get; set; } // A2 Weapon Sprite
        protected GameObject wa3 { get; set; } // A3 Weapon Sprite

        public SelectUI select;

        #region Unity Engine

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
        
        protected override void Update()
        {
            CurrentState = GetTop();
            CurrentState.Execute(this, Time.deltaTime);
            if (hasSpecialAttack)
            {
                currentSA.Execute(this, Time.deltaTime);
            }
            inputToState();
        }

        #endregion

        protected override void Initializing()
        {
            parentName = "Characters";
            
            AddState(new PlayerIdle());

            playerUI = transform.GetChild(1).GetComponent<PlayerUI>();

            mpValue = transform.GetChild(1).GetChild(1).GetComponent<TextMeshProUGUI>();
            
            LoadPlayer();
            
            radius = 0.5f;
        }

        protected virtual void LoadPlayer()
        {
            playerUI.InitalHearts(HP);
            UpdateUI();
        }

        #region Animations

        public override void StopAnimation()
        {
            SetAnimations(Motion.Idle);
        }

        public override void SetAnimations(int motion)
        {
            var index = GetFacing();
            var animStateInfo = animator.GetCurrentAnimatorStateInfo(0);
            var NTime = animStateInfo.normalizedTime;
            if (NTime > 0)
                animator.Play(animations[index + (4 * motion)].name);
            if (motion == Motion.Attack && projectile != null)
            {
                if (index == Direction.Down)
                {
                    ThrowProjectile(-0.128f,-0.495f);
                }
                if (index == Direction.Left)
                {
                    ThrowProjectile(-0.498f,-0.267f);
                }
                if (index == Direction.Right)
                {
                    ThrowProjectile(0.5f,-0.258f);
                }
                if (index == Direction.Up)
                {
                    ThrowProjectile(-0.184f,0.503f);
                }
            }
        }

        protected override void CalculateDirection()
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
            
            animator.SetInteger(Facing, facing);
        }
        
        #endregion

        #region UI

        private PlayerUI playerUI;

        protected override void UpdateUI()
        {
            playerUI.UpdateHeart(CHP);
            mpValue.text = "MP: " + CMP + "";
        }

        #endregion
        
        #region Stats Management
        
        public override void ChangeHeath(int healthValue)
        {
            HP += healthValue;
            if (CHP != 0)
            {
                // playerUI.InitalHearts(HP);
                UpdateUI();
            }
        }

        public override void ChangeRange(int rangeValue)
        {
            base.ChangeRange(rangeValue);
            childCollider.radius = RAN*5;
        }

        [SuppressMessage("ReSharper", "Unity.PerformanceCriticalCodeInvocation")]
        public override void TakeDamage(int dmg)
        {
            base.TakeDamage(dmg);
            
            if (CHP > 0) return;
            Debug.Log("You lost...");
            GameManager.instance.Lose();
        }

        #endregion
        
        #region States

        public override bool ConfigState()
        {
            if (states.Count > 1)
            {
                var temp = states.Pop();
                if (temp is BasicAttack && !hasSpecialAttack) return true;
                states.Pop();
                AddState(new PlayerIdle());
                return false;
            }
            states.Pop();
            AddState(new PlayerIdle());
            return false;
        }

        public override bool IsBasicAttack()
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
                    Target = t.transform.gameObject;
                    if (IsBasicAttack())
                    {
                        if (CurrentState.CurrentStatus is StateStatus.Initialize)
                        {
                            StateProgress();
                        } 
                    }
                    switch (select.isOn)
                    {
                        case true:
                        {
                            if (isSpecialAttack()) AddState(new WalkToLocation());
                            else if (!IsBasicAttack()) ChangeState(new WalkToLocation());
                            break;
                        }
                        case false:
                            select.ToggleOn();
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
                StateProgress();
            
            else if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                if (hasSpecialAttack) return;
                currentSA = A1();
                hasSpecialAttack = currentSA!=null;
            }
            
            else if (Input.GetKeyDown(KeyCode.Escape))
            {
                if(select!=null)
                {
                    select.ToggleOff();
                    if (IsBasicAttack()) ToComplete();
                }
            }
        }

        #endregion
    }
}