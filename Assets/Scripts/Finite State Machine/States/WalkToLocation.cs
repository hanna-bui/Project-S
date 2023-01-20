using Finite_State_Machine.Enemy_States;
using System.Collections.Generic;
using Characters.Enemy;
using UnityEngine;
using Characters;
using Movement;

namespace Finite_State_Machine.States
{
    public class WalkToLocation : State
    {
        private List<Vector3> roadPath;
        
        public WalkToLocation()
        {
            interval = 0f;
        }

        public override void Execute(MoveableObject agent)
        {
            var gm = agent.gm;
            var grid = gm.grid;

            if (CurrentStatus is StateStatus.Initialize)
            {
                var pos = agent.Position();
                var tar = agent.TargetLocation;

                roadPath = grid.CreatePath(pos, tar);

                switch (agent)
                {
                    case Character:
                        agent.SetAnimations(agent.CalculateDirection());
                        agent.animator.SetBool(gm.click, true);
                        break;
                    case Enemy:
                        agent.SetAnimations(Action.Jump);
                        break;
                }

                CurrentStatus = StateStatus.Executing;
            }

            if (CurrentStatus is StateStatus.Executing)
            {
                if (roadPath != null && roadPath.Count != 0)
                {
                    agent.TargetLocation = roadPath[0];

                    if (agent.IsAtPosition()) roadPath.RemoveAt(0);

                    if (roadPath.Count == 0)
                    {
                        roadPath = null;
                        CurrentStatus = StateStatus.Completed;
                    }

                    if (agent is Character)
                        agent.SetAnimations(agent.CalculateDirection());

                    Move(agent);
                }
            }

            if (CurrentStatus is StateStatus.Completed)
            {
                if (agent is Character)
                {
                    agent.StopAnimation();
                    agent.ChangeState(new PlayerIdle());
                }
                else
                {
                    agent.SetAnimations(Action.Idle);
                    agent.ChangeState(new EnemyIdle());
                }
            }
        }
        
        private static void Move(MoveableObject agent)
        {
            var pos = agent.Position();
            var tar = agent.TargetLocation;

            var speed = agent switch
            {
                Enemy enemy => enemy.SPE,
                Character player => player.SPE * 10,
                _ => 0f
            };
            
            var towards = Vector2.MoveTowards(pos, tar, speed * Time.deltaTime);
            agent.SetPosition(towards);
        }
    }
}