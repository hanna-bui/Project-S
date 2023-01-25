using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Goal_Behaviour
{
    public class CompositeGoal<T> : Goal<T>
    {
        protected List<Goal<T>> Subgoals { get; set; }
        
        public CompositeGoal(T owner) : base(owner)
        {
            Subgoals = new List<Goal<T>>();
        }

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
        
        public override void AddSubgoal(Goal<T> g)
        {
            Subgoals.Insert(0, g);
        }

        public ProcessOptions ProcessSubgoals()
        {
            while (Subgoals.Any() && (Subgoals[0].IsCompleted() || Subgoals[0].HasFailed()))
            {
                Subgoals[0].Terminate();
                Subgoals.RemoveAt(0);
            }

            if (Subgoals.Any())
            {
                var statusOfSubGoals = Subgoals[0].Process();

                if (statusOfSubGoals is ProcessOptions.Completed && Subgoals.Count() > 1)
                {
                    return ProcessOptions.Active;
                }

                return statusOfSubGoals;
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