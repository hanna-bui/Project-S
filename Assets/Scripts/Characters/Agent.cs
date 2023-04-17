using System.Collections.Generic;
using UnityEngine;
using System;
using Characters.Colliders;
using Characters.Enemies;
using Finite_State_Machine.States;
using Finite_State_Machine.States.Abilities;
using Movement.Pathfinding;
using UnityEngine.Serialization;
using State = Finite_State_Machine.State;

namespace Characters
{

    public class Agent : MonoBehaviour
    {
        protected CircleCollider2D childCollider;
        
        protected float radius;
        
        public GameObject Target { get; set; }

        public Vector3 TargetLocation { get; set; }
        
        protected new Camera camera;
        
        public bool isRespawning;
        
        public NewGrid grid;

        protected string parentName;

        private Vector3 origin;
        
        public GameObject projectile;
        
        public GameObject projectile2;
        
        // ReSharper disable once ConvertToAutoProperty
        public Vector3 Origin
        {
            get => origin;
            private set => origin = value;
        }

        protected virtual void Start()
        {
            grid = GameObject.FindGameObjectWithTag("Level").GetComponent<NewGrid>();

            states = new Stack<State>();

            camera = Camera.main;
            TargetLocation = transform.position;
            
            animator = GetComponent<Animator>();
            var ac = animator.runtimeAnimatorController;
            animations = ac.animationClips;
            Array.Sort(animations, new AnimationCompare());

            facing = 0;

            Initializing();
            
            SetParent();
            
            SetupCollider();
            var child = transform.GetChild(0).gameObject.GetComponent<RangeCollider>();
            childCollider = child.SetupCollider(RAN);

            var temp = transform;
            temp.localScale = Vector3.one;
            
            Origin = temp.position;
        }

        protected virtual void Initializing() { }

        protected virtual void Update()
        {
            CurrentState = GetTop();
            CurrentState.Execute(this);
        }
        
        private void SetupCollider()
        {
            var rig = gameObject.AddComponent<Rigidbody2D>();
            rig.bodyType = RigidbodyType2D.Kinematic;
            rig.simulated = true;

            var circle = gameObject.AddComponent<CircleCollider2D>();
            circle.radius = radius;
            circle.isTrigger = true;
        }

        private void SetParent()
        {
            var parent = GameObject.Find(parentName).transform;
            if (parent!=null) transform.SetParent(parent);
        }
        
        public bool IsAtPosition()
        {
            return transform.position.Equals(TargetLocation);
        }
        
        public bool IsAtPosition(Vector3 target)
        {
            return transform.position.Equals(target);
        }
        
        public bool IsAtTarget(Vector3 target)
        {
            return TargetLocation.Equals(target);
        }
        
        public void SetPosition(Vector3 newPosition)
        {
            transform.position = newPosition;
        }

        public Vector3 Position()
        {
            return transform.position;
        }

        #region Abilities

        /// <summary>
        /// Ability 1
        /// </summary>
        protected virtual State A1()
        {
            return null;
        }

        /// <summary>
        /// Ability 2
        /// </summary>
        protected virtual State A2()
        {
            return null;
        }

        #endregion

        #region Animation
        
        [SerializeField] protected AnimationClip[] animations;
        public Animator animator;
        protected int facing;
        protected static readonly int Facing = Animator.StringToHash("facing");

        [FormerlySerializedAs("FX1")] public GameObject fx1;
        [FormerlySerializedAs("FX2")] public GameObject fx2;
        
        public void EquipFX(GameObject agentFX1)
        {
            var fx = Instantiate(agentFX1);
            // fx.transform.SetParent(transform);
            fx.transform.position = Position();

            fx.GetComponent<AnimateFX>().Play();
        }

        public int GetFacing()
        {
            CalculateDirection();
            return facing;
        }
        
        public void SetFacing(int i)
        {
            facing = i;
        }

        protected virtual void CalculateDirection()
        {
            
        }

        public virtual void StopAnimation()
        {
            SetAnimations(facing);
        }
        
        public void SetExactAnimation(int motion)
        {
            var animStateInfo = animator.GetCurrentAnimatorStateInfo(0);
            var nTime = animStateInfo.normalizedTime;
            if (nTime > 0)
                animator.Play(animations[motion].name);
        }

        public virtual void SetAnimations(int index)
        {
            animator.Play(animations[index].name);
        }
        
        /// <summary>
        /// Helps For Sorting Animation Clips by Name
        /// </summary>
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

        protected virtual void UpdateUI()
        {
        }

        #endregion
        
        #region Stats & Its Management
        
        // ReSharper disable InconsistentNaming
        protected int HP { get; set; } // health points
        protected int CHP { get; set; } // current health points
        private int MP { get; set; } // mana points
        protected int CMP { get; private set; } // current mana points
        public int SPE { get; private set; } // speed
        protected int RAN { get; private set; } // range
        public int DMG { get; private set; } // damage
        private int MDMG { get; set; } // magic damage
        private int DEF { get; set; } // defense
        private int MDEF { get; set; } // magic defense
        protected int LVL { get; private set; } // level
        // ReSharper restore InconsistentNaming
        

        public void RestoreStats(List<int> stats)
        {
            SetupStats(stats[0], stats[1], stats[2], stats[3], stats[4], stats[5], stats[6], stats[7], stats[8]);
        }

        /// <summary>
        /// Current Level Stats, contains all of the stats of the player/enemy at their current level.
        /// </summary>
        /// <returns>List of Stats</returns>
        public List<int> CurrentStats()
        {
            return new List<int> { HP, MP, SPE, RAN, DMG, DEF, MDMG, MDEF, LVL };
        }

        public bool CanAttack()
        {
            return CHP > 0;
        }

        public bool NeedsHealing()
        {
            return CHP < HP;
    
        }

        public virtual void ChangeHeath(int healthValue)
        {
            HP += healthValue;
            if (CHP != 0)
            {
                UpdateUI();
            }
        }
        
        public void ChangeMana(int manaValue)
        {
            MP += manaValue;
            UpdateUI();
        }

        public void ChangeDamage(int damageValue)
        {
            DMG += damageValue;
        }
        
        public virtual void ChangeRange(int rangeValue)
        {
            RAN += rangeValue;
        }

        private void ChangeDefence(int defenceValue)
        {
            DEF += defenceValue;
        }

        private void ChangeMagicDefence(int magicDefenceValue)
        {
            SPE += magicDefenceValue;
        }

        private void ChangeSpeed(int speedValue)
        {
            MDEF += speedValue;
        }

        public void RestoreHealth(int hpRestore)
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
            UpdateUI();
        }

        private void SetupStats(int hp = 0, int mp = 0, int spe = 0, int ran = 0, int dmg = 0, int def = 0, int mdmg = 0, int mdef = 0, int lvl = 0)
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
            ChangeHeath(healthValue);
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
        
        public bool hasSpecialAttack;

        // ReSharper disable once InconsistentNaming
        protected State currentSA = null;

        protected Stack<State> states;

        public State CurrentState { get; protected set; }

        public void StateProgress()
        {
            CurrentState.StateProgress();
        }

        protected void ToComplete()
        {
            CurrentState.ToComplete();
        }

        protected State GetTop()
        {
            return states.Peek();
        }

        public virtual bool ConfigState()
        {
            return false;
        }
        
        public virtual bool IsBasicAttack()
        {
            return CurrentState is BasicAttack;
        }
        
        public bool IsWalkToLocation()
        {
            return CurrentState is WalkToLocation;
        }
        
        public void RemoveSpecial1(State oldSpecial1)
        {
            RemoveState(oldSpecial1);
            hasSpecialAttack = false;
        }

        private void RemoveState(State oldState)
        {
            if (!states.Contains(oldState)) return;
            
            if (oldState == states.Peek()) ConfigState();
            else
            {
                var tempStates = new Stack<State>();
                while (states.Count >= 1)
                {
                    var temp = states.Pop();
                    if (temp == oldState) break;
                    else tempStates.Push(temp);
                }
                while (tempStates.Count >= 1)
                {
                    AddState(tempStates.Pop());
                }
            }
        }
        
        public void AddState(State newState)
        {
            states.Push(newState);
            Debug.Log(newState.ToString());
        }

        public void ChangeState(State newState)
        {
            states.Pop();
            states.Push(newState);
            Debug.Log(newState.ToString());
        }

        #endregion
        
        #region OtherMethods

        protected void ThrowProjectile(float x, float y)
        {
            GameObject s;
            if (hasSpecialAttack)
            {
                currentSA.StateProgress();
                s = Instantiate(projectile2);
            }
            else
                s = Instantiate(projectile);
            s.GetComponent<Projectile>().Spawn(gameObject, GetFacing(), DMG, Target, new Vector3(x, y, 0));
        }
        
        public override string ToString()
        {
            return "HP = " + HP + ", CHP = " + CHP + ", MP = " + MP + ", CMP = " + CMP + ", DMG = " + DMG +
                   ", MDMG = " + MDMG + ", DEF = " + DEF + ", MDEF = " + DEF + ", RAN = " + RAN + ", SPE = " + SPE +
                   ", LVL = " + LVL;
        }

        #endregion
    }
}