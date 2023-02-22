using System.Collections;
using System.IO;
using System.Linq;
using Unity.VisualScripting;
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

        private ArrayList AllRooms = new ArrayList();


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
            OrderedPair p = new OrderedPair(i, j);
            Level[i,j] = RoomType.Start;
            AllRooms.Add(p);
            rooms++;
            CreateRooms(p);
            
            StreamWriter writer = new StreamWriter("arr.csv");
            for (i = 0; i < 10; i++)
            {
                for (j = 0; j < 10; j++) {
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
        private void CreateRooms(OrderedPair start)
        {
            //int i = 1;
            //int j = 1;
            OrderedPair[] points = new OrderedPair[4];
            points[0] = ChooseNewRoom(start);
            points[1] = ChooseNewRoom(start);
            points[2] = ChooseNewRoom(start);
            points[3] = ChooseNewRoom(start);
            // Initial 4 rooms must be valid, so they will all be set in ChooseNewRoom.
            OrderedPair room;
            while (rooms < totalRooms)
            {
                int[] dead = { 0, 0, 0, 0 };
                for (int n = 0; n < 4; n++)
                {
                    if (dead[n] == 1)
                        continue;
                    if ((room = ChooseNewRoom(points[n])).i != -1)
                        points[n] = room;
                    else
                    {
                        // todo: do something to continue generating from another point
                        Debug.Log("Direction " + n + " generation is done. [" + points[n].i + ", " + points[n].j + "]");
                        for (int m = 0; m < 4; m++)
                        {
                            if (n != m && (room = ChooseNewRoom(points[m])).i != -1)
                            {
                                points[n] = room;
                                break;
                            }
                            dead[n] = 1;
                            Debug.Log("Unable to find a viable room for point " + n + ". Generation  has been terminated.");
                        }
                            
                    }
                }
            }
        }

        /// <summary>
        /// Randomly chooses an Empty room adjacent to the input room.
        /// If the room choice is successful, it updates Level, AllRooms, and rooms.
        /// Input: OrderedPair p, the input room.
        /// Output: OrderedPair, a room adjacent to start.
        /// </summary>
        private OrderedPair ChooseNewRoom(OrderedPair p)
        {
            // validDirections: init at 1; set to 0 if room is found invalid to avoid redundant checks.
            int[] validDirections = { 1, 1, 1, 1 };
            // min and max: the range for random number generation.
            // min/max increased/decreased if a room is no longer valid to avoid generating a number in that range.
            int min = 1;
            int max = 4;
            do
            {
                int choice = Random.Range(min, max+1);
                switch (choice)
                {
                    case 1: // on top
                        // if room has not been tried before, is in bounds, and empty, it is valid and we set it up!
                        if (validDirections[0] == 1 && p.i - 1 >= 0 && Level[p.i - 1, p.j] == RoomType.Empty)
                        {
                            OrderedPair room = new OrderedPair(p.i - 1, p.j);
                            Level[room.i, room.j] = RoomType.Normal;
                            AllRooms.Add(room);
                            rooms++;
                            return room;
                        }
                        // if any conditions failed, it is invalid and we can not consider it anymore.
                        validDirections[0] =  0;
                        min++;
                        break;
                    case 2: // on right
                        if (validDirections[1] == 1 && p.j + 1 <= 9 && Level[p.i, p.j + 1] == RoomType.Empty)
                        {
                            OrderedPair room = new OrderedPair(p.i, p.j + 1);
                            Level[room.i, room.j] = RoomType.Normal;
                            AllRooms.Add(room);
                            rooms++;
                            return room;
                        }
                        validDirections[1] =  0;
                        if(min==2)
                            min++;
                        break;
                    case 3: // on bottom
                        if (validDirections[2] == 1 && p.i + 1 <= 9 && Level[p.i + 1, p.j] == RoomType.Empty)
                        {
                            OrderedPair room = new OrderedPair(p.i + 1, p.j);
                            Level[room.i, room.j] = RoomType.Normal;
                            AllRooms.Add(room);
                            rooms++;
                            return room;
                        }
                        validDirections[2] = 0;
                        if(max==3)
                            max--;
                        break;
                    case 4: // on left
                        if (validDirections[3] == 1 && p.j - 1 >= 0 && Level[p.i, p.j - 1] == RoomType.Empty)
                        {
                            OrderedPair room = new OrderedPair(p.i, p.j - 1);
                            Level[room.i, room.j] = RoomType.Normal;
                            AllRooms.Add(room);
                            rooms++;
                            return room;
                        }
                        validDirections[3] =  0;
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