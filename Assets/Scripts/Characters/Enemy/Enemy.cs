using Finite_State_Machine.Enemy_States;
using Finite_State_Machine.States;
using UnityEngine;
// ReSharper disable InconsistentNaming
// ReSharper disable IdentifierTypo
// ReSharper disable ConvertToAutoProperty
// ReSharper disable ConvertToAutoPropertyWhenPossible

namespace Characters.Enemy{

    public class Enemy : MoveableObject
    {
        [SerializeField] private bool isBoss;

        private float hp;
        private float chp;
        private float mp;
        private float cmp;
        private float spe;
        private float ran;
        private float dmg;
        private float mdmg;
        private float def;
        private float mdef;
        [SerializeField] private int lvl;

        private GameObject sprite;
        
        public enum MovementOptions
        {
            Plus,
            Side,
            Random,
            None
        };
        
        [SerializeField] private MovementOptions movementStyle = MovementOptions.Plus;

        protected bool IsBoss
        {
            get => isBoss;
            set => isBoss = value;
        }

        protected float HP
        {
            get => hp;
            set => hp = value;
        } // health points

        protected float CHP
        {
            get => chp;
            set => chp = value;
        } // current health points

        protected float MP
        {
            get => mp;
            set => mp = value;
        } // mana points

        protected float CMP
        {
            get => cmp;
            set => cmp = value;
        } // current mana points

        public float SPE
        {
            get => spe;
            set => spe = value;
        } // speed

        protected float RAN
        {
            get => ran;
            set => ran = value;
        } // range

        protected float DMG
        {
            get => dmg;
            set => dmg = value;
        } // damage

        protected float MDMG
        {
            get => mdmg;
            set => mdmg = value;
        } // magic damage

        protected float DEF
        {
            get => def;
            set => def = value;
        } // defense

        protected float MDEF
        {
            get => mdef;
            set => mdef = value;
        } // magic defense

        protected int LVL
        {
            get => lvl;
            set => lvl = value;
        } // level

        protected GameObject Sprite
        {
            get => sprite;
            set => sprite = value;
        } // Enemy Sprite

        public MovementOptions MovementStyle
        {
            get => movementStyle;
            set => MovementStyle = value;
        } // Enemy Sprite
        
        protected override void Start()
        {
            base.Start();
            
            HP = RandomStat();
            CHP = RandomStat();
            MP = RandomStat();
            CMP = RandomStat();
            SPE = Random.Range(1, 11);
            RAN = RandomStat();
            DMG = RandomStat();
            MDMG = RandomStat();
            DEF = RandomStat();
            MDEF = RandomStat();

            Sprite = gameObject;

            //weapon = ;
            //wa1 = ;
            //wa2 = ;
            //wa3 = ;
            
            Origin = transform.localPosition;
            SetupCollider();

            currentState = new PatternWalk();
        }

        // protected override void Update()
        // {
        //     base.Update();
        // }
        
        private void OnTriggerEnter2D(Collider2D col)
        {
            if (col.gameObject.CompareTag("Player"))
            {
                Debug.Log("Hitting Player's Collider");
                var player = col.gameObject.transform;
                TargetLocation = player.position;
                currentState = new WalkToLocation();
            }
        }
        private void OnTriggerExit2D(Collider2D col)
        {
            if (col.gameObject.CompareTag("Player"))
            {
                Debug.Log("Leaving Player's Collider");
                currentState = new PatternWalk();
            }
        }

        private void SetupCollider()
        {
            var rig = gameObject.AddComponent<Rigidbody2D>();
            rig.bodyType = RigidbodyType2D.Kinematic;
            rig.simulated = true;

            var circle = gameObject.AddComponent<CircleCollider2D>();
            circle.radius = RAN / 5;
            circle.isTrigger = true;
            
            Debug.Log(SPE);
        }

        protected float RandomStat()
        {
            return LVL * Random.Range(isBoss ? 7 : 1, 11);
        }
    }
}