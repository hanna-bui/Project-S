﻿using System;
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
        protected bool isABuff;
        protected float timeLimit;
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
                if (isABuff)
                    TimedExecute(agent, time);
                else
                    Execute(agent);
                time = 0;
            }
        }

        private void TimedExecute(MoveableObject agent, float timer)
        {
            if (timer > timeLimit)
            {
                ChangeStatus(StateStatus.Completed);
            }
            else
            {
                Execute(agent);
            }
        }

        public void Execute(MoveableObject agent) 
        {
            switch (CurrentStatus)
            {
                case StateStatus.Initialize:
                    Initialize(agent);
                    break;
                case StateStatus.Executing:
                    Executing(agent);
                    break;
                case StateStatus.Completed:
                    Completed(agent);
                    break;
                case StateStatus.Failed:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            } 
        }

        protected virtual void Initialize(MoveableObject agent) { }

        protected virtual void Executing(MoveableObject agent) { }

        protected virtual void Completed(MoveableObject agent) { }

        public void ChangeStatus(StateStatus status)
        {
            CurrentStatus = status;
        }
    }
}