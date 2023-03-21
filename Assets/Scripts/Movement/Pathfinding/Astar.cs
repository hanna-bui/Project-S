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

            var startRecord = new Record(start, null, 0f, Estimate(start, end), Category.Open, null);

            var recordList = new RecordList();
            recordList.Add(startRecord);

            var current = new Record(start);

            while (recordList.Count() > 0)
            {
                current = recordList.SmallestElement();

                if (current.node.Equals(end))
                    break;

                var connections = current.node.GetNeighbours();

                foreach (var connection in connections)
                {
                    var endNode = connection.GetToNode();
                    if (endNode == null) continue;
                    var endNodeCost = current.costSoFar + connection.GetCost();

                    Record endNodeRecord;
                    float endNodeHeuristic;

                    if (recordList.Contains(endNode, Category.Closed))
                    {
                        endNodeRecord = recordList.Find(endNode);

                        if (endNodeRecord.costSoFar <= endNodeCost) continue;
                        endNodeRecord.category = Category.Open;

                        endNodeHeuristic = endNodeRecord.estimatedTotalCost - endNodeRecord.costSoFar;
                    }
                    else if (recordList.Contains(endNode, Category.Open))
                    {
                        endNodeRecord = recordList.Find(endNode);

                        if (endNodeRecord.costSoFar <= endNodeCost) continue;

                        endNodeHeuristic = endNodeRecord.estimatedTotalCost - endNodeRecord.costSoFar;
                    }
                    else
                    {
                        endNodeRecord = new Record(endNode);
                        endNodeHeuristic = Estimate(endNode, end);
                    }

                    endNodeRecord.costSoFar = endNodeCost;
                    endNodeRecord.connection = connection;
                    endNodeRecord.estimatedTotalCost = endNodeCost + endNodeHeuristic;
                    endNodeRecord.prev = current;

                    if (!recordList.Contains(endNode, Category.Open))
                    {
                        endNodeRecord.category = Category.Open;
                        recordList.Add(endNodeRecord);
                    }
                }

                current.category = Category.Closed;
            }

            if (!current.node.Equals(end)) return null;

            var path = new List<Vector3>();
            while (!current.node.Equals(start))
            {
                var tempNode = current.connection.GetToNode();
                var targetLocation = floorMap.CellToWorld(new Vector3Int(tempNode.x, tempNode.y, 0));
                targetLocation.x += 10f;
                targetLocation.y += 10f;
                path.Add(targetLocation);
                current = current.prev;
            }

            path.Reverse();
            return path;
        }

        public float Estimate(Node fromNode, Node toNode)
        {
            return Vector2.Distance(fromNode.GetPosition(), toNode.GetPosition());
        }
    }
}