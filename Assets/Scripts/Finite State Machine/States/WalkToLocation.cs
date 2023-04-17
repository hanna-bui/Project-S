using System.Collections.Generic;
using UnityEngine;
using Characters;
using Characters.Enemies;
using Motion = Characters.Motion;

namespace Finite_State_Machine.States
{
    public class WalkToLocation : State
    {
        private List<Vector3> roadPath;
        
        public WalkToLocation()
        {
            interval = 0f;
        }
        
        protected override void Initialize(Agent agent) 
        {
            var grid = agent.grid;
            
            var pos = agent.Position();
            var tar = agent.TargetLocation;

            roadPath = grid.CreatePath(pos, tar);

            switch (agent)
            {
                case Character:
                    agent.SetAnimations(Motion.Walk);
                    break;
                case Boss:
                    agent.SetAnimations(BossAction.JUMP);
                    break;
                case Monster:
                    break;
            }

            CurrentStatus = StateStatus.Executing;
        }

        protected override void Executing(Agent agent)
        {
            if (roadPath != null && roadPath.Count != 0)
            {
                agent.TargetLocation = roadPath[0];
                if (agent is Monster)
                {
                    var facing = agent.GetFacing() + 1;
                    agent.SetExactAnimation(facing);
                }

                if (agent.IsAtPosition()) roadPath.RemoveAt(0);

                if (roadPath.Count == 0)
                {
                    roadPath = null;
                    CurrentStatus = StateStatus.Completed;
                }

                Move(agent);
            }
        }

        protected override void Completed(Agent agent)
        {
            agent.ConfigState();
        }
        
        private static void Move(Agent agent)
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