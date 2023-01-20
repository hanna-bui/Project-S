using Characters;
using UnityEngine;

// ReSharper disable ConvertToAutoProperty

namespace Finite_State_Machine
{
    public enum StateStatus
    {
        Initialize,
        Executing,
        Completed,
        Failed
    }
    
    public class State
    {
        private StateStatus currentStatus = StateStatus.Initialize;
        protected float interval;
        private float time = 0f;

        public State()
        {
            interval = 3f;
        }

        public StateStatus CurrentStatus
        {
            get => currentStatus;
            set => currentStatus = value;
        } // level
        
        public void Execute(MoveableObject agent, float addTime)
        {
            time += addTime;
            if (time > interval)
            {
                Execute(agent);
                time = 0;
            }
        }
        
        public virtual void Execute(MoveableObject agent)
        {
            
        }

        public void ChangeStatus(StateStatus status)
        {
            CurrentStatus = status;
        }
    }
}