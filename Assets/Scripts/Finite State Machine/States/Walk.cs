using System.Collections.Generic;
using UnityEngine;
using Characters;

namespace Finite_State_Machine.States
{
    public class Walk : State
    {
        private List<Vector3> roadPath;

        public override void Execute(MoveableObject agent)
        {
            var gm = agent.gm;
            var grid = gm.grid;
            
            if (currentStatus is StateStatus.Initialize)
            {
                var pos = agent.Position();
                var tar = agent.TargetLocation;
                
                roadPath = grid.CreatePath(pos, tar);

                agent.SetAnimations();
                agent.animator.SetBool(gm.click, true);
                
                currentStatus = StateStatus.Executing;
            }
            
            if (currentStatus is StateStatus.Executing)
            {
                if (roadPath != null && roadPath.Count != 0)
                {
                    agent.TargetLocation = roadPath[0];

                    if (agent.IsAtPosition()) roadPath.RemoveAt(0);

                    if (roadPath.Count == 0)
                    {
                        roadPath = null;
                        currentStatus = StateStatus.Completed;
                    }
                    agent.SetAnimations();
                    Move(agent);
                }
            }

            if (currentStatus is StateStatus.Completed)
            {
                agent.StopAnimation();
                agent.ChangeState(new Idle());
            }
        }
        
        private static void Move(MoveableObject agent)
        {
            var pos = agent.Position();
            var tar = agent.TargetLocation;
            
            var distance = agent.Speed * 10 * Time.deltaTime;
            var towards = Vector2.MoveTowards(pos, tar, distance);
            agent.SetPosition(towards);
        }
    }
}