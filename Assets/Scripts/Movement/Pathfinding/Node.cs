using System.Collections.Generic;
using UnityEngine;

namespace Movement.Pathfinding
{
    public class Node
    {
        private readonly Dictionary<Node, float> neighbours;
        public readonly int x;
        public readonly int y;

        public Node(int x, int y)
        {
            this.x = x;
            this.y = y;
            neighbours = new Dictionary<Node, float>();
        }

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

                    neighbours.Add(grid[i, j], cost);
                }
            }
        }
        
        public Dictionary<Node, float> GetNeighbours()
        {
            return neighbours;
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
    
        public class NodeRecord
        {
            public NodeRecord(Node node)
            {
                this.ThisNode = node;
            }

            public NodeRecord(Node node, Node next, float costToNext, float totalCost, float estimatedTotalCost,
                Category category, NodeRecord prev)
            {
                this.ThisNode = node;
                this.NextNode = next;
                this.CostToNext = costToNext;
                this.TotalCost = totalCost;
                this.EstimatedTotalCost = estimatedTotalCost;
                this.Configuration = category;
                this.PrevRecord = prev;
            }

            public Category Configuration { get; set; }

            public float TotalCost { get; private set; }

            public float EstimatedTotalCost { get; private set; }

            public Node ThisNode { get; }

            public NodeRecord PrevRecord { get; private set; }

            public Node NextNode { get; private set; }

            private float CostToNext { get; set; }

            public void Add(float total, Node next, float costToNext, float estimate, NodeRecord prev)
            {
                this.TotalCost = total;
                this.NextNode = next;
                this.CostToNext = costToNext;
                this.EstimatedTotalCost = estimate;
                this.PrevRecord = prev;
            }

            public bool Equals(NodeRecord obj)
            {
                return ThisNode.Equals(obj.ThisNode);
            }

            public override string ToString()
            {
                return ThisNode + ", connection = ( Next Node = " + NextNode + ", Cost to Next = " + CostToNext + "), costSoFar = " + TotalCost + ", category = " + Configuration;
            }
        }
    }
}
