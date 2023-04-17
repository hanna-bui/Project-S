using System.Collections.Generic;
using UnityEngine;

namespace Characters.Enemies
{
    public static class BossAction
    {
        public const int ATTACK = 0;
        public const int CHARGE = 1;
        public const int HIT = 2;
        public const int IDLE = 3;
        public const int JUMP = 4;
    }
    
    public class Boss : Enemy
    {
        protected override void Initializing()
        {
            MovementStyle = MovementOptions.Side;
            
            var hp = RandomStat();
            var mp = RandomStat();
            var spe = Random.Range(5, 10);
            var ran = Random.Range(6, 16);
            var dmg = RandomStat();
            var def = RandomStat();
            var mdmg = RandomStat();
            var mdef = RandomStat();
            
            RestoreStats(new List<int> { hp, mp, spe, ran, dmg, def, mdmg, mdef, lvl });

            agroRange = 12;
            
            radius = 1f;
            
            base.Initializing();
        }
        
        protected override int RandomStat()
        {
            return LVL * Random.Range(6, 16);
        }
    }
}