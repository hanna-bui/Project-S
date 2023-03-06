using System.Collections.Generic;
using Finite_State_Machine;
using Finite_State_Machine.Enemy_States;
using Movement;
using TMPro;
using UnityEngine;

// ReSharper disable InconsistentNaming
// ReSharper disable IdentifierTypo
// ReSharper disable ConvertToAutoProperty
// ReSharper disable ConvertToAutoPropertyWhenPossible

namespace Characters.Enemy{
    
    public static class Action
    {
        public const int Idle = 0;
        public const int Attack = 1;
        public const int Charge = 2;
        public const int Hit = 3;
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
        [SerializeField] private TextMeshProUGUI hpValue;

        
        protected override void Start()
        {
            base.Start();
            
            LVL = lvl;

            HP = RandomStat();
            CHP = HP;
            MP = RandomStat();
            CMP = CMP;
            SPE = Random.Range(1, 11);
            RAN = RandomStat();
            DMG = RandomStat();
            MDMG = RandomStat();
            DEF = RandomStat();
            MDEF = RandomStat();

            Sprite = gameObject;

            radius = 1f;
            SetupCollider();
            
            Origin = transform.position;
            
            States = new Stack<State>();
            States.Push(new PatternWalk());
            
            hpValue.text = "" + CHP + "";
        }

        protected override void Update()
        {
            currentState = GetTop();
            currentState.Execute(this);
            
            if (CHP <= 0)
            {
                Destroy(gameObject);
            }
        }
        
        private void OnTriggerEnter2D(Collider2D col)
        {
            if (col.gameObject.CompareTag("Player"))
            {
                var player = col.gameObject.transform;
                ChangeState(new EnemyIdle());
            }
        }
        private void OnTriggerExit2D(Collider2D col)
        {
            if (col.gameObject.CompareTag("Player"))
            {
                ChangeState(new PatternWalk());
            }
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

        private float RandomStat()
        {
            return LVL * Random.Range(IsBoss ? 20 : 10, 31);
        }
        
        public override void TakeDamage(float dmg)
        {
            CHP -= dmg;
            hpValue.text = "" + CHP + "";
        }
    }
}