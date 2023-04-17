using System.Collections.Generic;
using Characters.Colliders;
using Finite_State_Machine.Enemy_States;
using UnityEngine;
using Random = UnityEngine.Random;

// ReSharper disable InconsistentNaming
// ReSharper disable IdentifierTypo
// ReSharper disable ConvertToAutoProperty
// ReSharper disable ConvertToAutoPropertyWhenPossible

namespace Characters.Enemies{

    public class Enemy : Agent
    {
        public enum MovementOptions
        {
            Plus,
            Side,
            Random,
            None
        };
        
        // [SerializeField] public bool IsBoss = false;
        [SerializeField] public MovementOptions MovementStyle = MovementOptions.Plus;
        public int lvl { get; set; }
        // [SerializeField] public float scale = 1;
        protected static int agroRange = 12;

        public SelectUI select;

        protected override void Initializing()
        {
            parentName = "Enemies";
            
            AddState(new PatternWalk());
            
            var stats = new List<int> { Random.Range(1, 11), Random.Range(1, 11), Random.Range(5, 10), 
                Random.Range(3, 8), Random.Range(1, 3), Random.Range(1, 11), Random.Range(1, 11), Random.Range(6, 11), lvl };
            
            RestoreStats(stats);
            
            var child = transform.GetChild(2).gameObject.GetComponent<AgroCollider>();
            child.SetupCollider(agroRange);
            
            UpdateUI();
            
            select = transform.GetChild(3).gameObject.GetComponent<SelectUI>();
        }

        protected override void Update()
        {
            CurrentState = GetTop();
            CurrentState.Execute(this, Time.deltaTime);
        }
        
        public override bool ConfigState()
        {
            if (NeedsHealing())
                AddState(new EnemyHeal());
            else
                AddState(new PatternWalk());
            return false;
        }

        public override bool IsBasicAttack()
        {
            return CurrentState is EnemyAttack;
        }

        private bool isHealing()
        {
            return CurrentState is EnemyHeal;
        }
        
        #region Stats Management
        
        /// <summary>
        /// Nonika:
        /// I made the stat range specific to enemy type
        /// So regular enemies get 15-25, bosses get 25-35.
        /// Regular stats:
        /// return LVL * Random.Range(IsBoss ? 25 : 15, IsBoss ? 36 : 26);
        /// Stats based on enemy size:
        /// </summary>
        /// <returns></returns>
        protected virtual int RandomStat()
        {
            return 0;
        }
        
        public override void TakeDamage(int dmg)
        {
            base.TakeDamage(dmg);
            if (CHP > 0) SetAnimations(BossAction.HIT);
            else Destroy(gameObject);
        }

        #endregion
    }
}