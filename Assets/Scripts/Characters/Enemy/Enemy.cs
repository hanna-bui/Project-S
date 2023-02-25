using Finite_State_Machine.Enemy_States;
using Finite_State_Machine.States;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// ReSharper disable InconsistentNaming
// ReSharper disable IdentifierTypo
// ReSharper disable ConvertToAutoProperty
// ReSharper disable ConvertToAutoPropertyWhenPossible

namespace Characters.Enemy{

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
            currentState = new PatternWalk();
            
            hpValue.text = "" + CHP + "";
        }

        protected override void Update()
        {
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
                currentState = new EnemyIdle();
            }
        }
        private void OnTriggerExit2D(Collider2D col)
        {
            if (col.gameObject.CompareTag("Player"))
            {
                currentState = new PatternWalk();
            }
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