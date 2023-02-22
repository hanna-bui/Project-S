using System.Collections;
using System.IO;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace RoomGen
{
    /// <summary>
    /// Generator responsible for creating the layout of the entire level.
    /// </summary>
    [DisallowMultipleComponent]
    public class LevelGenerator : MonoBehaviour
    {
        public enum RoomType : byte
        {
            Empty,
            Hall,
            Normal,
            Special,
            Chest,
            Start,
            End
        }

        /// <summary>
        /// The Level Matrix.
        /// </summary>
        public RoomType[,] Level = new RoomType[10,10];

        [Header("Room Variables")] [Tooltip("How many rooms will be generated.")] [SerializeField]
        private int totalRooms = 30;

        private int rooms = 0;


        // Start is called before the first frame update
        void Start()
        {
            int i = 0;
            int j = 0;
            for (; i < 10; i++)
            {
                for (; j < 10; j++)
                {
                    Level[i, j] = RoomType.Empty;
                }
            }
            /* The starting room can spawn anywhere except for the 2 cells closest to the edges.
             This keeps the starting room random but prevents early performance issues by eliminating an edge or corner
             starting room. In this case, one or two room spawn directions would be invalid from the start, and the
             algorithm would be constantly trying to find a new valid direction for rooms to spawn in.
             */
            i = Random.Range(2, 7);
            j = Random.Range(2, 7);
            Level[i,j] = RoomType.Start;
            rooms++;
            CreateRooms(new OrderedPair(i, j));
            
            StreamWriter writer = new StreamWriter("arr.csv");
            for (; i < 10; i++)
            {
                for (; j < 10; j++) {
                    if (Level[i,j] == RoomType.Empty)
                        writer.Write(",");
                    else
                        writer.Write(Level[i,j] + ",");
                }
                writer.Write(System.Environment.NewLine);
            }
            writer.Flush();
            writer.Close();
            
        }

        /// <summary>
        /// Generator responsible for creating all the rooms.
        /// It loops until the generated room count (rooms) hits the desired total room count (totalRooms).
        /// </summary>
        private void CreateRooms(OrderedPair p)
        {
            int i = 1;
            int j = 1;
            OrderedPair[] directions = new OrderedPair[4];
            directions[0] = ChooseRoom(p);
            directions[1] = ChooseRoom(p);
            directions[2] = ChooseRoom(p);
            directions[3] = ChooseRoom(p);
            // Initial 4 rooms must be valid, so we can set them without checking.
            foreach (OrderedPair d in directions)
            {
                Level[d.i, d.j] = RoomType.Normal;
                rooms++;
            }
            OrderedPair room;
            while (rooms < totalRooms)
            {
                for (int n = 0; n < 4; n++)
                {
                    if ((room = ChooseRoom(directions[n])).i == -1)
                        // todo: do something to continue generating from another point
                        Debug.Log("Direction "+ n+ " generation is done.");
                    else
                    {
                        directions[n] = room;
                        Level[room.i, room.j] = RoomType.Normal;
                        rooms++;
                    }
                }
            }
        }

        /// <summary>
        /// Randomly chooses an Empty room adjacent to the input room.
        /// Input: OrderedPair p, the input room.
        /// Output: OrderedPair, a room adjacent to p.
        /// </summary>
        private OrderedPair ChooseRoom(OrderedPair p)
        {
            // validDirections: init at 1, 2, 3, 4; set to -1 if room is found invalid to avoid redundant checks.
            int[] validDirections = { 1, 2, 3, 4 };
            // min and max: the range for random number generation.
            // min/max increased/decreased if a room is no longer valid to avoid generating a number in that range.
            int min = 1;
            int max = 4;
            do
            {
                int choice = Random.Range(min, max);
                switch (choice)
                {
                    case 1: // on top
                        // if room has not been tried before
                        if (validDirections[0] != -1 && p.i - 1 >= 0 && Level[p.i - 1, p.j] == RoomType.Empty)
                            return new OrderedPair(p.i - 1, p.j);
                        validDirections[0] = -1;
                        min++;
                        break;
                    case 2: // on right
                        if (validDirections[1] != -1 && p.j + 1 <= 9 && Level[p.i, p.j + 1] == RoomType.Empty)
                            return new OrderedPair(p.i - 1, p.j);
                        validDirections[1] = -1;
                        if(min==2)
                            min++;
                        break;
                    case 3: // on bottom
                        if (validDirections[2] != -1 && p.i + 1 <= 9 && Level[p.i + 1, p.j] == RoomType.Empty)
                            return new OrderedPair(p.i - 1, p.j);
                        validDirections[2] = -1;
                        if(max==3)
                            max--;
                        break;
                    case 4: // on left
                        if (validDirections[3] != -1 && p.j - 1 >= 0 && Level[p.i, p.j - 1] == RoomType.Empty)
                            return new OrderedPair(p.i - 1, p.j);
                        validDirections[3] = -1;
                        max--;
                        break;
                }
            } while (min <= max && validDirections[0] != -1 && validDirections[1] != -1 && validDirections[2] != -1 && validDirections[3] != -1);

            // Will hit this statement if all rooms are taken!
            return new OrderedPair(-1, -1);
        }
    }

    /// <summary>
    /// OrderedPair class for easily returning ordered pairs!
    /// </summary>
    public class OrderedPair
    {
        public int i;
        public int j;

        public OrderedPair(int i, int j)
        {
            this.i = i;
            this.j = j;
        }
    }
}