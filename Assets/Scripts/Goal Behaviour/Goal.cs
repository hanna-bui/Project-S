using System.Collections.Generic;
using UnityEngine;

namespace Goal_Behaviour
{
    public class Goal<T>
    {
        public enum ProcessOptions
        {
            Inactive,
            Active,
            Completed,
            Failed
        };

        protected T Owner { get; set; }
        protected ProcessOptions CurrentStatus { get; set; }

        public Goal(T owner)
        {
            this.Owner = owner;
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

        public virtual void AddSubgoal(Goal<T> g)
        {
            
        }

        protected void ActivateIfInactive()
        {
            if (CurrentStatus is ProcessOptions.Inactive) 
                Activate();
        }

        public bool IsActive()
        {
            return this.CurrentStatus == ProcessOptions.Active;
        }
        
        public bool IsCompleted()
        {
            return this.CurrentStatus == ProcessOptions.Completed;
        }
        
        public bool HasFailed()
        {
            return this.CurrentStatus == ProcessOptions.Failed;
        }
    }
}