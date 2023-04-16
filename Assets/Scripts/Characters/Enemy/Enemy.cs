using System;
using System.Collections.Generic;
using Finite_State_Machine;
using Finite_State_Machine.Enemy_States;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

// ReSharper disable InconsistentNaming
// ReSharper disable IdentifierTypo
// ReSharper disable ConvertToAutoProperty
// ReSharper disable ConvertToAutoPropertyWhenPossible

namespace Characters.Enemy{
    
    public static class Action
    {
        public const int Attack = 0;
        public const int Charge = 1;
        public const int Hit = 2;
        public const int Idle = 3;
        public const int Jump = 4;
    }

    public class Enemy : MoveableObject
    {
        public enum MovementOptions
        {
            Plus,
            Side,
            Random,
            None
        };
        
        [SerializeField] public bool IsBoss = false;
        [SerializeField] public MovementOptions MovementStyle = MovementOptions.Plus;
        [SerializeField] private int lvl = 1;
        [SerializeField] public float scale = 1;
        
        
        protected override void Start()
        {
            base.Start();
            
            LVL = lvl;

            HP = RandomStat();
            CHP = HP;
            MP = RandomStat();
            CMP = CMP;
            SPE = Random.Range(1, 11);
            RAN = Random.Range(6, 11);
            DMG = Random.Range(1, 3);
            MDMG = RandomStat();
            DEF = RandomStat();
            MDEF = RandomStat();

            radius = 1f;
            SetupCollider();
            var child = transform.GetChild(0).gameObject.GetComponent<EnemyCollider>();
            child.SetupCollider(RAN);

            Origin = transform.position;
            
            hpValue.text = "HP: " + CHP + "";
            
            
            transform.parent = GameObject.Find("Enemies").transform;
            //transform.localScale = new Vector3(1, 1, 1);
            /* Nonika:
             * I made the scale a variable so I can fit more enemies in a room by making them smaller.
             */
            transform.localScale = new Vector3(scale, scale, 1);
            
            States = new Stack<State>();
            SetAnimations(Action.Idle);
            States.Push(new PatternWalk());
        }

        protected override void Update()
        {
            CurrentState = GetTop();
            CurrentState.Execute(this, Time.deltaTime);
        }
        
        public override bool IsSubState()
        {
            if (States.Count > 1)
            {
                States.Pop();
                return true;
            }
            States.Pop();
            SetAnimations(Action.Idle);
            States.Push(new EnemyIdle());
            return false;
        }

        #region Stats Management

        private int RandomStat()
        {
            //return LVL * Random.Range(IsBoss ? 20 : 15, 31);
            /* Nonika:
             * I made the stat range specific to enemy type
             * So regular enemies get 15-25, bosses get 25-35.
             * Regular stats:
             * return LVL * Random.Range(IsBoss ? 25 : 15, IsBoss ? 36 : 26);
             * Stats based on enemy size:
             */
            return (int)(LVL * scale * Random.Range(IsBoss ? 25 : 15, IsBoss ? 36 : 26));
        }
        
        public override void TakeDamage(int dmg)
        {
            base.TakeDamage(dmg);
            if (CHP > 0) SetAnimations(Action.Hit);
            else Destroy(gameObject);
        }

        #endregion
        
    }
}