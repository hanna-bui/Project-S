using System.Collections.Generic;
using UnityEngine;

namespace Characters.Enemies
{
    public static class MonsterAction
    {
        public const int IDLE = 0;
        public const int DOWN = 1;
        public const int LEFT = 2;
        public const int RIGHT = 3;
        public const int UP = 4;
    }
    
    public class Monster : Enemy
    {
        protected override void Initializing()
        {
            MovementStyle = MovementOptions.Side;
            
            var hp = RandomStat();
            var mp = RandomStat();
            var spe = Random.Range(5, 10);
            var ran = Random.Range(5, 7);
            var dmg = RandomStat();
            var def = RandomStat();
            var mdmg = RandomStat();
            var mdef = RandomStat();
            
            RestoreStats(new List<int> { hp, mp, spe, ran, dmg, def, mdmg, mdef, lvl });

            agroRange = 9;
            
            radius = 0.5f;
            
            base.Initializing();
        }
        
        public override void SetAnimations(int motion)
        {
            var index = GetFacing();
            var animStateInfo = animator.GetCurrentAnimatorStateInfo(0);
            var nTime = animStateInfo.normalizedTime;
            if (nTime > 0)
                animator.Play(animations[index + (4 * motion)].name);
        }
        
        protected override void CalculateDirection()
        {
            var position = transform.position;
            
            var p1 = new Vector2(position.x, position.y);
            var p2 = new Vector2(TargetLocation.x, TargetLocation.y);

            var angleFloat = Mathf.Atan2(p2.y - p1.y, p2.x - p1.x) * 180 / Mathf.PI;
            var angleInt = Mathf.CeilToInt(angleFloat);

            facing = angleInt switch
            {
                <= 135 and >= 45 => Direction.Up,
                < 45 and > -45 => Direction.Right,
                <= -45 and >= -135 => Direction.Down,
                _ => Direction.Left
            };
        }
        
        protected override int RandomStat()
        {
            return LVL * Random.Range(1, 7);
        }
        
    }
}