using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;
// ReSharper disable FieldCanBeMadeReadOnly.Global

namespace Movement.Pathfinding
{
    public class Astar
    {
        public enum Category
        {
            Closed,
            Open,
            Unvisited
        }

        private Node end;
        private Heuristic heuristic;
        private Node start;

        public List<Vector3> PathfindingAstar(Node startingNode, Node endingNode, Tilemap floorMap)
        {
            start = startingNode;
            end = endingNode;
            heuristic = new Heuristic(end);

            var startRecord = new NodeRecord(start, null, 0f, heuristic.Estimate(start), Category.Open, null);

            var recordList = new PathfindingList();
            recordList.Add(startRecord);

            var current = new NodeRecord(start);

            while (recordList.Count() > 0)
            {
                current = recordList.SmallestElement();

                if (current.node.Equals(end))
                    break;

                var connections = current.node.GetConnections();

                foreach (var connection in connections)
                {
                    var endNode = connection.GetToNode();
                    if (endNode == null) continue;
                    var endNodeCost = current.costSoFar + connection.GetCost();

                    NodeRecord endNodeRecord;
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
                        endNodeRecord = new NodeRecord(endNode);
                        endNodeHeuristic = heuristic.Estimate(endNode);
                    }

                    endNodeRecord.costSoFar = endNodeCost;
                    endNodeRecord.connection = connection;
                    endNodeRecord.estimatedTotalCost = endNodeCost + endNodeHeuristic;
                    endNodeRecord.previousNodeRecord = current;

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
                current = current.previousNodeRecord;
            }

            path.Reverse();
            return path;
        }

        public class NodeRecord
        {
            public Category category;
            public Connection connection;
            public float costSoFar;
            public float estimatedTotalCost;
            public Node node;
            public NodeRecord previousNodeRecord;

            public NodeRecord(Node node)
            {
                this.node = node;
            }

            public NodeRecord(Node node, Connection connection, float costSoFar, float estimatedTotalCost,
                Category category, NodeRecord previousNodeRecord)
            {
                this.node = node;
                this.connection = connection;
                this.costSoFar = costSoFar;
                this.estimatedTotalCost = estimatedTotalCost;
                this.category = category;
                this.previousNodeRecord = previousNodeRecord;
            }

            public bool Equals(NodeRecord obj)
            {
                return node.Equals(obj.node);
            }

            public override string ToString()
            {
                return node + ", connection = (" + connection + "), costSoFar = " + costSoFar + ", category = " + category;
            }
        }

        public class Connection
        {
            private readonly float cost;
            private Node fromNode;
            private readonly Node toNode;

            public Connection(float cost, Node fromNode, Node toNode)
            {
                this.cost = cost;
                this.fromNode = fromNode;
                this.toNode = toNode;
            }

            public float GetCost()
            {
                return cost;
            }

            public void SetFromNode(Node newFromNode)
            {
                this.fromNode = newFromNode;
            }

            public Node GetFromNode()
            {
                return fromNode;
            }

            public Node GetToNode()
            {
                return toNode;
            }

            public override string ToString()
            {
                return "(" + fromNode + " -> " + toNode + ")";
            }
        }

        private class Heuristic
        {
            private readonly Node goalNode;

            public Heuristic(Node goalNode)
            {
                this.goalNode = goalNode;
            }

            public float Estimate(Node fromNode)
            {
                return Vector2.Distance(fromNode.GetPosition(), goalNode.GetPosition());
            }
        }

        public class Node
        {
            private readonly List<Connection> neighbours;
            public int x;
            public int y;

            public Node(int x, int y)
            {
                this.x = x;
                this.y = y;
                neighbours = new List<Connection>();
            }

            public void AddNeighbours(Node[,] grid, Node thisNode)
            {
                if (x < grid.GetUpperBound(0) && grid[x + 1, y]!=null)
                    neighbours.Add(new Connection(1, thisNode, grid[x + 1, y]));
                if (x > 0 && grid[x - 1, y]!=null)
                    neighbours.Add(new Connection(1, thisNode, grid[x - 1, y]));
                if (y < grid.GetUpperBound(1) && grid[x, y + 1]!=null)
                    neighbours.Add(new Connection(1, thisNode, grid[x, y + 1]));
                if (y > 0 && grid[x, y - 1]!=null)
                    neighbours.Add(new Connection(1, thisNode, grid[x, y - 1]));

                #region diagonal

                if (x > 0 && y > 0 && grid[x - 1, y - 1]!=null)
                    neighbours.Add(new Connection(1.4f, thisNode, grid[x - 1, y - 1]));
                if (x < grid.GetLength(0) - 1 && y > 0 && grid[x + 1, y - 1]!=null)
                    neighbours.Add(new Connection(1.4f, thisNode, grid[x + 1, y - 1]));
                if (x > 0 && y < grid.GetLength(1) - 1 && grid[x - 1, y + 1]!=null)
                    neighbours.Add(new Connection(1.4f, thisNode, grid[x - 1, y + 1]));
                if (x < grid.GetLength(0) - 1 && y < grid.GetLength(1) - 1 && grid[x + 1, y + 1]!=null)
                    neighbours.Add(new Connection(1.4f, thisNode, grid[x + 1, y + 1]));

                #endregion diagonal
            }

            public List<Connection> GetConnections()
            {
                return neighbours;
            }

            public HashSet<Node> GetToNodes()
            {
                var toNodes = new HashSet<Node>();
                foreach (var c in neighbours) toNodes.Add(c.GetToNode());
                return toNodes;
            }

            public Vector2 GetPosition()
            {
                return new Vector2(x, y);
            }

            public bool Equals(Node obj)
            {
                return x == obj.x && y == obj.y;
            }

            public override string ToString()
            {
                return "x = " + x + ", y =" + y;
            }
        }

        private class PathfindingList
        {
            private readonly List<NodeRecord> pfList;

            public PathfindingList()
            {
                pfList = new List<NodeRecord>();
            }

            public void Add(NodeRecord node)
            {
                pfList.Add(node);
            }

            public NodeRecord SmallestElement()
            {
                var smallest = pfList[0];
                foreach (var nr in pfList.Where(nr => nr.estimatedTotalCost <= smallest.estimatedTotalCost && nr.category==Category.Open))
                    smallest = nr;
                
                if (smallest.Equals(pfList[0]))
                    foreach (var nr in pfList.Where(nr => nr.category == Category.Open))
                    {
                        smallest = nr;
                        break;
                    }
                return smallest;
            }

            public int Count()
            {
                return pfList.Count;
            }

            public bool Contains(Node node, Category category)
            {
                return pfList.Any(nodeRecord => (nodeRecord.node.Equals(node) && nodeRecord.category==category));
            }

            public NodeRecord Find(Node node)
            {
                return pfList.FirstOrDefault(nodeRecord => nodeRecord.node.Equals(node));
            }

/*
            public void Remove(NodeRecord node)
            {
                pfList.Remove(node);
            }
*/
        }
    }
}