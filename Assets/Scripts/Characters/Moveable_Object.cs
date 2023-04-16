using System.Collections.Generic;
using UnityEngine;
using System;
using Movement.Pathfinding;
using TMPro;
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


        /// public healthBar healthbar;

        public Vector3 TargetLocation { get; set; }
        
        protected new Camera camera;

        protected TextMeshProUGUI hpValue;
        
        public bool isRespawning;
        
        
        protected virtual void Start()
        {
            grid = GameObject.FindGameObjectWithTag("Level").GetComponent<NewGrid>();
            ///healthbar.setMaxHealth(HP);

            States = new Stack<State>();

            camera = Camera.main;
            TargetLocation = transform.position;
            
            animator = GetComponent<Animator>();
            var ac = animator.runtimeAnimatorController;
            animations = ac.animationClips;
            Array.Sort(animations, new AnimationCompare());

            facing = 0;
            
            hpValue = transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>();
        }

        protected virtual void Update()
        {
            CurrentState = GetTop();
            CurrentState.Execute(this);
        }

        #region Animation
        
        [SerializeField] protected AnimationClip[] animations;
        public Animator animator;
        protected int facing;
        
        public virtual void CalculateDirection(){}
        

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

        #region UI

        public virtual void UpdateUI()
        {
            hpValue.text = "HP: " + CHP + "";
            ///healthbar.setHealth(CHP);
        }

        #endregion
        
        #region Stats & Its Management
        
        [SerializeField] public int HP { get; set; } // health points
        protected int CHP { get; set; } // current health points
        protected int MP { get; set; } // mana points
        protected int CMP { get; set; } // current mana points
        public int SPE { get; set; } // speed
        protected int RAN { get; set; } // range
        public int DMG { get; set; } // damage
        protected int MDMG { get; set; } // magic damage
        protected int DEF { get; set; } // defense
        protected int MDEF { get; set; } // magic defense
        protected int LVL { get; set; } // level

        public void RestoreStats(List<int> stats)
        {
            SetupStats(stats[0], stats[1], stats[2], stats[3], stats[4], stats[5], stats[6], stats[7], stats[8]);
        }

        /// <summary>
        /// Current Level Stats, contains all of the stats of the player/enemy at their current level.
        /// </summary>
        /// <returns>List of Stats</returns>
        public List<int> CLS()
        {
            return new List<int> { HP, MP, SPE, RAN, DMG, DEF, MDMG, MDEF, LVL };
        }

        public bool isAttackable()
        {
            return CHP > 0;
        }

        public bool NeedsHealing()
        {
            return CHP < HP;
    
        }

        public void ChangeHP(int healthValue)
        {
            HP += healthValue;
            if (CHP != 0) UpdateUI();
        }
        
        public void ChangeMana(int manaValue)
        {
            MP += manaValue;
            if (CHP != 0) UpdateUI();
        }
        
        public void ChangeDamage(int damageValue)
        {
            DMG += damageValue;
        }
        
        public void ChangeRange(int rangeValue)
        {
            RAN += rangeValue;
        }
        
        public void ChangeDefence(int defenceValue)
        {
            DEF += defenceValue;
        }
        
        public void ChangeMagicDefence(int magicDefenceValue)
        {
            SPE += magicDefenceValue;
        }
        
        public void ChangeSpeed(int speedValue)
        {
            MDEF += speedValue;
        }

        public void RestoreHP(int hpRestore)
        {
            CHP = Math.Min(CHP + hpRestore, HP);
            UpdateUI();
        }

        public void RestoreMana(int manaRestore)
        {
            CMP = Math.Min(CMP + manaRestore, MP);
            UpdateUI();
        }

        public virtual void TakeDamage(int dmg)
        {
            CHP = Math.Max(CHP - dmg, 0);
            if (CHP != 0) UpdateUI();
        }
        
        public void SetupStats(int hp = 0, int mp = 0, int spe = 0, int ran = 0, int dmg = 0, int def = 0, int mdmg = 0, int mdef = 0, int lvl = 0)
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

        public void LevelUp(int healthValue = 0, int manaValue = 0, int damageValue = 0, int rangeValue = 0, int defenceValue = 0, int magicDefenceValue = 0, int speedValue = 0)
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
        
        #endregion

        #region State

        protected Stack<State> States;

        public State CurrentState { get; set; }

        public NewGrid grid;
        
        private Vector3 origin;
        
        // ReSharper disable once ConvertToAutoProperty
        public Vector3 Origin
        {
            get => origin;
            set => origin = value;
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

        #endregion
        
        #region OtherMethods
        
        protected void SetupCollider()
        {
            var rig = gameObject.AddComponent<Rigidbody2D>();
            rig.bodyType = RigidbodyType2D.Kinematic;
            rig.simulated = true;

            var circle = gameObject.AddComponent<CircleCollider2D>();
            circle.radius = radius;
            circle.isTrigger = true;
        }

        protected virtual void Respawn()
        {
            var spawnPt = GameObject.Find("SpawnPoint").transform;
            transform.position = spawnPt.position;
        }
        
        public int GetFacing()
        {
            return facing;
        }
        
        public bool IsAtPosition()
        {
            return transform.position.Equals(TargetLocation);
        }
        
        public bool IsAtPosition(Vector3 target)
        {
            return transform.position.Equals(target);
        }
        
        public void SetPosition(Vector3 newPosition)
        {
            transform.position = newPosition;
        }

        public Vector3 Position()
        {
            return transform.position;
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