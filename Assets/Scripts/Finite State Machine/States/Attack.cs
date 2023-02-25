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

        private WalkToLocation temp = new WalkToLocation();

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
                    if (enemyList.Contains(Target))
                        TargetStat = enemyList[Target] as Enemy;
                    
                    interval = DefaultInterval;
                    break;
                case StateStatus.Executing:
                    if (TargetStat is not null)
                    {
                        switch (TargetStat.CHP)
                        {
                            case > 0 when InRange(agent):
                            {
                                TargetStat.TakeDamage(agent.DMG);
                                break;
                            }
                            case < 0:
                                ChangeStatus(StateStatus.Completed);
                                break;
                        }
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

        private bool InRange(MoveableObject agent)
        {
            var distance = Vector2.Distance(TargetStat.Position(), agent.Position());
            if (distance > agent.RAN)
            {
                agent.TargetLocation = TargetStat.Position();
                interval = 0f;
                temp.Execute(agent);
                return false;
            }

            interval = DefaultInterval;
            if (temp.CurrentStatus is StateStatus.Executing)
            {
                agent.StopAnimation();
                temp = new WalkToLocation();
            }

            return true;
        }
    }
}