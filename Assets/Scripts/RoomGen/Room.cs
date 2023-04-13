using UnityEngine;

namespace RoomGen
{
    public class Room
    {
        public readonly OrderedPair pair;
        public LevelGenerator.RoomType rt;
        public DD dd;
        public GameObject g;

        public Room()
        {
            this.pair = new OrderedPair(0, 0);
            this.rt = LevelGenerator.RoomType.Empty;
            this.dd = DD.E;
        }

        public Room(OrderedPair pair_, LevelGenerator.RoomType rt_, DD dd_)
        {
            this.pair = pair_;
            this.rt = rt_;
            this.dd = dd_;
        }
    
        public int i => pair.i;

        public int j => pair.j;

        public override string ToString()
        {
            return $"{nameof(rt)}: {rt}, {nameof(dd)}: {dd}, {nameof(i)}: {i}, {nameof(j)}: {j}";
        }
    }
}