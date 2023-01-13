// using System.Collections.Generic;
// using System.Linq;
// using Characters;
// using Movement.Pathfinding;
// using UnityEngine;
//
// using NewGrid = Movement.Pathfinding.NewGrid;
//
// namespace Goal_Behaviour.Player_s_Goals
// {
//     public class FollowPath : CompositeGoal<Character>
//     {
//         private List<AnimatorOverrideController> overrideControllers;
//         private Vector3 desiredLocation;
//         private NewGrid grid;
//         private List<Vector3> roadPath;
//         private List<PathEdge> pathEdges;
//         
//         public FollowPath(Character owner, Vector3 desiredLocation, NewGrid grid, List<AnimatorOverrideController> overrideControllers) : base(owner)
//         {
//             this.desiredLocation = desiredLocation;
//             this.grid = grid;
//             this.overrideControllers = overrideControllers;
//
//             pathEdges = new List<PathEdge>();
//             SetupPath();
//         }
//         
//         public override void Activate()
//         {
//             CurrentStatus = ProcessOptions.Active;
//             
//             PathEdge edge = pathEdges[0];
//             pathEdges.RemoveAt(0);
//
//             switch (edge.GetBehaviour())
//             {
//                 case BehaviourType.Normal:
//                     AddSubgoal(new TraverseEdge(Owner, edge));
//                     break;
//                 case BehaviourType.TippyToe:
//                     AddSubgoal(new TraverseEdge(Owner, edge));
//                     break;
//                 case BehaviourType.Run:
//                     AddSubgoal(new TraverseEdge(Owner, edge));
//                     break;
//             }
//         }
//
//         public override ProcessOptions Process()
//         {
//             ActivateIfInactive();
//
//             var status = ProcessSubgoals();
//             if (status is ProcessOptions.Completed && pathEdges.Any())
//             {
//                 Activate();
//             }
//             return status;
//         }
//
//         public override void Terminate()
//         {
//             
//         }
//
//         private void SetupPath()
//         {
//             roadPath = grid.CreatePath(Owner.GetPosition(), desiredLocation);
//             roadPath.Insert(0, Owner.GetPosition());
//             
//             for (var i = 1; i < roadPath.Count; i++)
//             {
//                 var source = roadPath[i-1];
//                 var destination = roadPath[i];
//                 var edge = new PathEdge(source, destination);
//                 
//                 pathEdges.Add(edge);
//             }
//         }
//     }
// }