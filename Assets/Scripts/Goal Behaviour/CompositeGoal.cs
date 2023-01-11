using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

namespace Goal_Behaviour
{
    public class CompositeGoal : Goal
    {
        public override void Activate()
        {
            
        }

        public override ProcessOptions Process()
        {
            var status = Subgoals[0].Process();
            
            return status;
        }

        public override void Terminate()
        {
            
        }

        public override bool HandleMessage(string message)
        {
            var bResult = Subgoals[0].HandleMessage(message);

            if (!bResult)
            {
                Debug.Log(message);
            }
            return bResult;
        }
        
        public override void AddSubgoal(Goal g)
        {
            Subgoals.Insert(0, g);
        }

        public ProcessOptions ProcessSubgoals()
        {
            while (Subgoals.Any() && Subgoals[0].Process() is ProcessOptions.Completed or ProcessOptions.Failed)
            {
                Subgoals[0].Terminate();
                Subgoals.RemoveAt(0);
            }

            if (Subgoals.Any())
            {
                var StatusOfSubGoals = Subgoals[0].Process();

                if (StatusOfSubGoals is ProcessOptions.Completed && Subgoals.Count() > 1)
                {
                    return ProcessOptions.Active;
                }

                return StatusOfSubGoals;
            }
            else
            {
                return ProcessOptions.Completed;
            }
        }
        
        public void RemoveAllSubgoals()
        {
            Subgoals.Clear();
        }
    }
}