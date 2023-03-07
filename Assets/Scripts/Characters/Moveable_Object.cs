using System.Collections.Generic;
using Finite_State_Machine;
using UnityEngine;
using Movement;
using System;
using Managers;
using Mirror;

// ReSharper disable IdentifierTypo

// ReSharper disable MemberCanBeProtected.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable InconsistentNaming

namespace Characters
{

    public class MoveableObject : NetworkBehaviour
    {
        protected float radius;

        public float HP { get; set; } // health points
        public float CHP { get; set; } // current health points
        protected float MP { get; set; } // mana points
        protected float CMP { get; set; } // current mana points
        public float SPE { get; set; } // speed
        public float RAN { get; set; } // range
        public float DMG { get; set; } // damage
        protected float MDMG { get; set; } // magic damage
        protected float DEF { get; set; } // defense
        protected float MDEF { get; set; } // magic defense
        [SerializeField] protected int LVL { get; set; } // level
        protected GameObject Sprite { get; set; } // Enemy Sprite
        
        [SerializeField] protected List<AnimatorOverrideController> overrideControllers;
        public Animator animator;

        public Vector3 TargetLocation { get; set; }
        
        protected new Camera camera;
        
        
        #region State

        protected State currentState;

        public GameManager gm;
        
        private Vector3 origin;
        
        // ReSharper disable once ConvertToAutoProperty
        public Vector3 Origin
        {
            get => origin;
            set => origin = value;
        }

        #endregion
        
        
        protected virtual void Start()
        {
            gm = GameObject.Find("GameManager").GetComponent<GameManager>();

            camera = Camera.main;
            TargetLocation = transform.position;
            animator = GetComponent<Animator>();
        }

        protected virtual void Update()
        {
            currentState.Execute(this);
        }

        protected void SetupCollider()
        {
            var rig = gameObject.AddComponent<Rigidbody2D>();
            rig.bodyType = RigidbodyType2D.Kinematic;
            rig.simulated = true;

            var circle = gameObject.AddComponent<CircleCollider2D>();
            circle.radius = radius;
            circle.isTrigger = true;
        }
        
        public bool IsAtPosition()
        {
            return transform.position.Equals(TargetLocation);
        }
        
        public bool IsAtPosition(Vector3 target)
        {
            return transform.position.Equals(target);
        }
        
        public int CalculateDirection()
        {
            var position = transform.position;
            if (Math.Abs(TargetLocation.x - position.x) < 0.00001) return 0;
            if (Math.Abs(TargetLocation.y - position.y) < 0.00001) return 0;

            var heading = (Vector2)TargetLocation - (Vector2)position;
            var magnitude = heading / heading.magnitude;
            var x = (decimal)magnitude.x;
            var y = (decimal)magnitude.y;
            
            if (Math.Max(Math.Abs(y), Math.Abs(x)) == Math.Abs(y))
            {
                return y > 0 ? Direction.Up : Direction.Down;
            }
            return x > 0 ? Direction.Right : Direction.Left;
        }
        
        #region Animation

        public void StopAnimation()
        {
            animator.SetBool(gm.click, false);
        }

        public void SetAnimations(int index)
        {
            var overrideController = overrideControllers[index];
            animator.runtimeAnimatorController = overrideController;
        }

        #endregion Animation

        #region Getters and Setters

        public void ChangeHP(float healthValue)
        {
            HP += healthValue;
        }
        
        public void ChangeMana(float manaValue)
        {
            MP += manaValue;
        }
        
        public void ChangeDamage(float damageValue)
        {
            DMG += damageValue;
        }
        
        public void ChangeRange(float rangeValue)
        {
            RAN += rangeValue;
        }
        
        public void ChangeDefence(float defenceValue)
        {
            DEF += defenceValue;
        }
        
        public void ChangeMagicDefence(float magicDefenceValue)
        {
            SPE += magicDefenceValue;
        }
        
        public void ChangeSpeed(float speedValue)
        {
            MDEF += speedValue;
        }

        public void RestoreHP(float hpRestore)
        {
            CHP = Math.Min(CHP + hpRestore, HP);
        }

        public void RestoreMana(float manaRestore)
        {
            CMP = Math.Min(CMP + manaRestore, MP);
        }

        public virtual void TakeDamage(float dmg)
        {
            CHP -= dmg;
        }

        public void LevelUp(float healthValue = 0, float manaValue = 0, float damageValue = 0, float rangeValue = 0, float defenceValue = 0, float magicDefenceValue = 0, float speedValue = 0)
        {
            ChangeHP(healthValue);
            ChangeMana(manaValue);
            ChangeDamage(damageValue);
            ChangeRange(rangeValue);
            ChangeDefence(defenceValue);
            ChangeMagicDefence(magicDefenceValue);
            ChangeSpeed(speedValue);
            LVL += 1;
        }

        public void ChangeState(State newState)
        {
            currentState = newState;
        }
        
        public void SetPosition(Vector3 newPosition)
        {
            transform.position = newPosition;
        }

        public Vector3 Position()
        {
            return transform.position;
        }
        
        public Vector3 LocalPosition()
        {
            return transform.localPosition;
        }
        
        public void SetLocalPosition(Vector3 newPosition)
        {
            transform.localPosition = newPosition;
        }

        #endregion

        public override string ToString()
        {
            return "HP = " + HP + ", CHP = " + CHP + ", MP = " + MP + ", CMP = " + CMP + ", DMG = " + DMG +
                   ", MDMG = " + MDMG + ", DEF = " + DEF + ", MDEF = " + DEF + ", RAN = " + RAN + ", SPE = " + SPE +
                   ", LVL = " + LVL;
        }
    }
}