using System.Collections.Generic;
using UnityEngine;
using Movement;
using System;
using Finite_State_Machine.States;
using Managers;
using Unity.VisualScripting;
using UnityEditor.Animations;
using State = Finite_State_Machine.State;

// ReSharper disable IdentifierTypo

// ReSharper disable MemberCanBeProtected.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable InconsistentNaming

namespace Characters
{

    public class MoveableObject : MonoBehaviour
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
        
        [SerializeField] protected AnimationClip[] animations;
        public Animator animator;
        protected int facing;

        public Vector3 TargetLocation { get; set; }
        
        protected new Camera camera;
        
        
        #region State

        protected Stack<State> States;

        public State CurrentState { get; set; }

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
            var ac = animator.runtimeAnimatorController;
            animations = animator.runtimeAnimatorController.animationClips;
            Array.Sort(animations, new AnimationCompare());

            facing = 0;
        }

        protected virtual void Update()
        {
            CurrentState = GetTop();
            CurrentState.Execute(this);
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
        
        #region Animation
        public virtual void CalculateDirection(){}
        
        public virtual void DirectionFromTarget(){}

        public virtual void StopAnimation()
        {
            SetAnimations(facing);
        }

        public virtual void SetAnimations(int index)
        {
            animator.Play(animations[index].name);
        }
        
        class AnimationCompare : IComparer<AnimationClip>
        {
            public int Compare(AnimationClip x, AnimationClip y)
            {
                if (x == null || y == null)
                {
                    return 0;
                }
          
                // CompareTo() method
                return string.Compare(x.name, y.name, StringComparison.Ordinal);
          
            }
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
        
        public void SetupStats(float hp = 0, float mp = 0, float spe = 0, float ran = 0, float dmg = 0, float def = 0, float mdmg = 0, float mdef = 0, int lvl = 0)
        {
            HP = hp;
            CHP = hp;
            MP = mp;
            CMP = mp;
            SPE = spe;
            RAN = ran;
            DMG = dmg;
            DEF = def;
            MDMG = mdmg;
            MDEF = mdef;
            LVL = lvl;
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

        public int GetFacing()
        {
            return facing;
        }
        
        public State GetTop()
        {
            return States.Peek();
        }

        public virtual bool IsSubState()
        {
            return false;
        }
        
        public void AddState(State newState)
        {
            States.Push(newState);
        }

        public void ChangeState(State newState)
        {
            States.Pop();
            States.Push(newState);
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