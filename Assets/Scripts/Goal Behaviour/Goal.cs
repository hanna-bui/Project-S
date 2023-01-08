using System.Collections.Generic;
using UnityEngine;

namespace Goal_Behaviour
{
    public class Goal : MonoBehaviour
    {
        public enum ProcessOptions
        {
            Inactive,
            Active,
            Completed,
            Failed
        };

        protected ProcessOptions CurrentStatus { get; set; }

        protected List<Goal> Subgoals { get; set; }

        public Goal()
        {
            this.Subgoals = new List<Goal>();
            this.CurrentStatus = ProcessOptions.Inactive;
        }
        public virtual void Activate()
        {
            
        }

        public virtual ProcessOptions Process()
        {
            return CurrentStatus;
        }

        public virtual void Terminate()
        {
            
        }

        public virtual bool HandleMessage(string message)
        {
            return true;
        }

        public virtual void AddSubgoal(Goal g)
        {
            
        }
    }
}