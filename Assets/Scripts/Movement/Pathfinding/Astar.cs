using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Record = Movement.Pathfinding.Node.NodeRecord;

// ReSharper disable FieldCanBeMadeReadOnly.Global

namespace Movement.Pathfinding
{
    public class Astar
    {
        private Node end;
        private Node start;

        public List<Vector3> PathfindingAstar(Node startingNode, Node endingNode, Tilemap floorMap)
        {
            start = startingNode;
            end = endingNode;

            var startRecord = new Record(start, null, 0f, 0f, Estimate(start, end), Category.Open, null);

            var recordList = new RecordList();
            recordList.Add(startRecord);

            var current = new Record(start);

            while (recordList.Count() > 0)
            {
                current = recordList.SmallestElement();

                if (current.ThisNode.Equals(end))
                    break;

                var connections = current.ThisNode.GetNeighbours();

                foreach (var (endNode, cost) in connections)
                {
                    if (endNode == null) continue;
                    var endNodeCost = current.TotalCost + cost;

                    Record endNodeRecord;
                    float endNodeHeuristic;

                    if (recordList.Contains(endNode, Category.Closed))
                    {
                        endNodeRecord = recordList.Find(endNode);

                        if (endNodeRecord.TotalCost <= endNodeCost) continue;
                        endNodeRecord.Configuration = Category.Open;

                        endNodeHeuristic = endNodeRecord.EstimatedTotalCost - endNodeRecord.TotalCost;
                    }
                    else if (recordList.Contains(endNode, Category.Open))
                    {
                        endNodeRecord = recordList.Find(endNode);

                        if (endNodeRecord.TotalCost <= endNodeCost) continue;

                        endNodeHeuristic = endNodeRecord.EstimatedTotalCost - endNodeRecord.TotalCost;
                    }
                    else
                    {
                        endNodeRecord = new Record(endNode);
                        endNodeHeuristic = Estimate(endNode, end);
                    }
                    
                    endNodeRecord.Add(endNodeCost, endNode, cost, endNodeCost + endNodeHeuristic, current);

                    if (recordList.Contains(endNode, Category.Open)) continue;
                    endNodeRecord.Configuration = Category.Open;
                    recordList.Add(endNodeRecord);
                }

                current.Configuration = Category.Closed;
            }

            if (!current.ThisNode.Equals(end)) return null;

            var path = new List<Vector3>();
            while (!current.ThisNode.Equals(start))
            {
                var tempNode = current.NextNode;
                var targetLocation = floorMap.CellToWorld(new Vector3Int(tempNode.x, tempNode.y, 0));
                targetLocation.x += 10f;
                targetLocation.y += 10f;
                path.Add(targetLocation);
                current = current.PrevRecord;
            }

            path.Reverse();
            return path;
        }

        private static float Estimate(Node fromNode, Node toNode)
        {
            return Vector2.Distance(fromNode.GetPosition(), toNode.GetPosition());
        }
    }
}