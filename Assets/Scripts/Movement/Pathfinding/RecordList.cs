using System.Collections.Generic;
using System.Linq;

using Record = Movement.Pathfinding.Node.NodeRecord;

namespace Movement.Pathfinding
{
    public class RecordList
    {
        private readonly List<Record> records;

        public RecordList()
        {
            records = new List<Record>();
        }

        public void Add(Record node)
        {
            records.Add(node);
        }

        public Record SmallestElement()
        {
            var smallest = records[0];
            foreach (var nr in records.Where(nr => nr.estimatedTotalCost <= smallest.estimatedTotalCost && nr.category==Category.Open))
                smallest = nr;
                
            if (smallest.Equals(records[0]))
                foreach (var nr in records.Where(nr => nr.category == Category.Open))
                {
                    smallest = nr;
                    break;
                }
            return smallest;
        }

        public int Count()
        {
            return records.Count;
        }

        public bool Contains(Node node, Category category)
        {
            return records.Any(nodeRecord => (nodeRecord.node.Equals(node) && nodeRecord.category==category));
        }

        public Record Find(Node node)
        {
            return records.FirstOrDefault(nodeRecord => nodeRecord.node.Equals(node));
        }

        // public void Remove(NodeRecord node)
        // {
        //     pfList.Remove(node);
        // }

    }
}