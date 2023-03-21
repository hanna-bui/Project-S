using System.Collections.Generic;
using UnityEngine;

namespace Movement.Pathfinding
{
    public class Node
    {
        private readonly List<Connection> neighbours;
        private readonly Dictionary<Node, float> s;
        public readonly int x;
        public readonly int y;

        public Node(int x, int y)
        {
            this.x = x;
            this.y = y;
            neighbours = new List<Connection>();
            s = new Dictionary<Node, float>();
        }
        
        //
        // Old code
        //
        // public void AddNeighbours(Node[,] grid, Node thisNode)
        // {
        //     if (x < grid.GetUpperBound(0) && grid[x + 1, y] != null)
        //         neighbours.Add(new Connection(1, thisNode, grid[x + 1, y]));
        //     if (x > 0 && grid[x - 1, y] != null)
        //         neighbours.Add(new Connection(1, thisNode, grid[x - 1, y]));
        //     if (y < grid.GetUpperBound(1) && grid[x, y + 1] != null)
        //         neighbours.Add(new Connection(1, thisNode, grid[x, y + 1]));
        //     if (y > 0 && grid[x, y - 1] != null)
        //         neighbours.Add(new Connection(1, thisNode, grid[x, y - 1]));
        //
        //     #region diagonal
        //
        //     if (x > 0 && y > 0 && grid[x - 1, y - 1] != null)
        //         neighbours.Add(new Connection(1.4f, thisNode, grid[x - 1, y - 1]));
        //     if (x < grid.GetLength(0) - 1 && y > 0 && grid[x + 1, y - 1] != null)
        //         neighbours.Add(new Connection(1.4f, thisNode, grid[x + 1, y - 1]));
        //     if (x > 0 && y < grid.GetLength(1) - 1 && grid[x - 1, y + 1] != null)
        //         neighbours.Add(new Connection(1.4f, thisNode, grid[x - 1, y + 1]));
        //     if (x < grid.GetLength(0) - 1 && y < grid.GetLength(1) - 1 && grid[x + 1, y + 1] != null)
        //         neighbours.Add(new Connection(1.4f, thisNode, grid[x + 1, y + 1]));
        //
        //     #endregion diagonal
        // }
        
        // Optimized
        public void AddNeighbours(Node[,] grid, Node thisNode)
        {
            var maxX = grid.GetLength(0) - 1;
            var maxY = grid.GetLength(1) - 1;
            for (var i = Mathf.Max(thisNode.x - 1, 0); i <= Mathf.Min(thisNode.x + 1, maxX); i++)
            {
                for (var j = Mathf.Max(thisNode.y - 1, 0); j <= Mathf.Min(thisNode.y + 1, maxY); j++)
                {
                    if (i == thisNode.x && j == thisNode.y)
                        continue;

                    if (grid[i, j] == null) continue;
                    var cost = 1f;
                    if (Mathf.Abs(i - thisNode.x) == 1 && Mathf.Abs(j - thisNode.y) == 1)
                        cost = 1.4f;

                    neighbours.Add(new Connection(cost, thisNode, grid[i, j]));
                }
            }
        }

        public List<Connection> GetNeighbours()
        {
            return neighbours;
        }

        // public HashSet<Node> GetToNodes()
        // {
        //     var toNodes = new HashSet<Node>();
        //     foreach (var c in neighbours) toNodes.Add(c.GetToNode());
        //     return toNodes;
        // }

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
    
        public class NodeRecord
        {
            public Category category;
            public Connection connection;
            public float costSoFar;
            public float estimatedTotalCost;
            public readonly Node node;
            public NodeRecord prev;

            public NodeRecord(Node node)
            {
                this.node = node;
            }

            public NodeRecord(Node node, Connection connection, float costSoFar, float estimatedTotalCost,
                Category category, NodeRecord prev)
            {
                this.node = node;
                this.connection = connection;
                this.costSoFar = costSoFar;
                this.estimatedTotalCost = estimatedTotalCost;
                this.category = category;
                this.prev = prev;
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
    }
}
