using UnityEngine;

namespace Movement.Pathfinding
{
    public enum BehaviourType
    {
        Normal,
        TippyToe,
        Run
    };
    public class PathEdge
    {
        private Vector3 source;
        private Vector3 destination;

        private BehaviourType behaviour;

        public PathEdge(Vector3 source, Vector3 destination, BehaviourType behaviour = BehaviourType.Normal)
        {
            this.source = source;
            this.destination = destination;
            this.behaviour = behaviour;
        }

        public Vector3 GetDestination()
        {
            return destination;
        }
        
        public BehaviourType GetBehaviour()
        {
            return behaviour;
        }
    }
}