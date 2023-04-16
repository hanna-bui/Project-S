using System;
using Characters.Enemy;
using Characters;
using UnityEngine;
using Motion = Characters.Motion;

// ReSharper disable PossibleNullReferenceException

namespace Finite_State_Machine.States
{

    public class Attack : State
    {
        private GameObject Target { get; set; }
        private Enemy TargetStat { get; set; }

        private const float DefaultInterval = 1.2f;

        public Attack(GameObject target)
        {
            interval = 0f;
            Target = target;
        }

        public override void Execute(MoveableObject agent)
        {
            switch (CurrentStatus)
            {
                case StateStatus.Initialize:
                    if (TargetStat is null)
                    {
                        TargetStat = Target.GetComponent<Enemy>();
                        agent.TargetLocation = TargetStat.Position();
                        agent.CalculateDirection();
                        interval = DefaultInterval;
                    }
                    break;
                case StateStatus.Executing:
                    if (TargetStat is not null)
                    {
                        agent.SetAnimations(Motion.Attack);
                        TargetStat.TakeDamage(agent.DMG);
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