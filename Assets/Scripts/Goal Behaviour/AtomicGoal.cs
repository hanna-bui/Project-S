using System.Collections;
using System.Collections.Generic;
using Goal_Behaviour;
using UnityEngine;

namespace Goal_Behaviour
{
    public class AtomicGoal : Goal
    {
        public override void Activate()
        {
            
        }

        public override ProcessOptions Process()
        {
            return CurrentStatus;
        }

        public override void Terminate()
        {
            
        }

        public override bool HandleMessage(string message)
        {
            return true;
        }
    }
}
