// using Movement.Pathfinding;
// using UnityEngine;
//
// namespace Goal_Behaviour.Player_s_Goals{
//
//     public class TraverseEdge : AtomicGoal<Character>
//     {
//         private readonly PathEdge edge;
//
//         private readonly bool isLastEdge;
//
//         private bool isStuck;
//         public TraverseEdge(Character owner, PathEdge edge, bool isLastEdge = false) : base(owner)
//         {
//             this.edge = edge;
//             this.isLastEdge = isLastEdge;
//         }
//
//         public override void Activate()
//         {
//             CurrentStatus = ProcessOptions.Active;
//             Owner.GetSteering().SetTarget(edge.GetDestination());
//
//             Owner.GetSteering().Seek();
//         }
//
//         public override ProcessOptions Process()
//         {
//             ActivateIfInactive();
//             if (isStuck)
//                 CurrentStatus = ProcessOptions.Failed;
//             else if (Owner.IsAtPosition(edge.GetDestination()))
//                 CurrentStatus = ProcessOptions.Completed;
//             return CurrentStatus;
//         }
//
//         public override void Terminate()
//         {
//             
//         }
//     }
// }