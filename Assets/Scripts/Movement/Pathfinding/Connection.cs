namespace Movement.Pathfinding
{
    public class Connection
    {
        private readonly float cost;
        private readonly Node fromNode;
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

        // public void SetFromNode(Node newFromNode)
        // {
        //     this.fromNode = newFromNode;
        // }
        //
        // public Node GetFromNode()
        // {
        //     return fromNode;
        // }

        public Node GetToNode()
        {
            return toNode;
        }

        public override string ToString()
        {
            return "(" + fromNode + " -> " + toNode + ")";
        }
    }
}