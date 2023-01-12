using UnityEngine;

namespace Movement.Steering_Behaviour
{

    public class SteeringBehaviour : MonoBehaviour
    {
        private Character owner;
        private Vector2 targetPos;
        public SteeringBehaviour(Character owner)
        {
            this.owner = owner;
        }
        
        public Vector2 Seek()
        {
            var desiredVelocity = (targetPos - owner.GetPosition()).normalized;
            return desiredVelocity - owner.GetVelocity();
        }
        
        public Vector2 FollowPath(Vector2 position)
        {
            return default;
        }
        
        public Vector2 Flee(Vector2 position)
        {
            return default;
        }
        
        public Vector2 Arrive(Vector2 position)
        {
            return default;
        }
        
        public Vector2 Calculate()
        {
            return default;
        }

        public Vector2 ForwardComponent()
        {
            return default;
        }

        public Vector2 SideComponent()
        {
            return default;
        }

        public void SetPath()
        {

        }

        public void SetTarget(Vector2 targetPos)
        {
            this.targetPos = targetPos;
        }
        
        public void SeekOn()
        {
            Seek();
        }
        
        public void FollowPathOn()
        {
        }
        
        public void FleeOn()
        {
        }
        
        public void ArriveOn()
        {
        }
        
        public void SeekOff()
        {
        }
        
        public void FollowPathOff()
        {
        }
        
        public void FleeOff()
        {
        }
        
        public void ArriveOff()
        {
        }
    }

}