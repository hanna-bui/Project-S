using System;
using Characters.Enemy;
using Characters;
using UnityEngine;

namespace Finite_State_Machine.States
{

    public class Attack : State
    {
        public GameObject Target { get; set; }
        private Enemy TargetStat { get; set; }

        private const float DefaultInterval = 3f;

        public Attack(MoveableObject agent, GameObject target)
        {
            interval = 0f;
            Target = target;
        }

        public override void Execute(MoveableObject agent)
        {
            var enemyList = agent.gm.Enemies;
            
            switch (CurrentStatus)
            {
                case StateStatus.Initialize:
                    if (TargetStat is null)
                    {
                        if (enemyList.Contains(Target))
                            TargetStat = enemyList[Target] as Enemy;
                    
                        interval = DefaultInterval;
                    }
                    break;
                case StateStatus.Executing:
                    if (TargetStat is not null && TargetStat.CHP > 0)
                    {
                        TargetStat.TakeDamage(agent.DMG);
                        ChangeStatus(StateStatus.Initialize);
                    }
                    else
                    {
                        ChangeStatus(StateStatus.Completed);
                    }
                    break;
                case StateStatus.Completed:
                    agent.ChangeState(new PlayerIdle());
                    break;
                case StateStatus.Failed:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}