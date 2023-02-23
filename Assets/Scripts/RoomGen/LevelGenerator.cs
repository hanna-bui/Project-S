using System.Collections;
using System.IO;
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
        public RoomType[,] Level = new RoomType[10, 10];

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
            Level[i, j] = RoomType.Start;
            AllRooms.Add(p);
            rooms++;
            CreateRooms(p);
            // todo: figure out what type of room each one is (i.e. which way doors face)
            // todo: use this classification to set up a boss room and other special rooms!

            StreamWriter writer = new StreamWriter("arr.csv");
            for (i = 0; i < 10; i++)
            {
                for (j = 0; j < 10; j++)
                {
                    if (Level[i, j] == RoomType.Empty)
                        writer.Write(",");
                    else
                        writer.Write(Level[i, j] + ",");
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
            /* This method works by keeping 4 "spawn points" which have the potential to spawn a room.
             * Each iteration, each point tries to choose a new room.
             * If the room returned is at (-1, -1), then none of the adjacent rooms are valid for spawning a new room.
             * In this case, the spawn point is considered not valid, and the algorithm will try to set its location
             * to another randomly-selected room which hopefully has valid adjacent rooms.
             */
            OrderedPair[] points = new OrderedPair[4];
            points[0] = ChooseNewRoom(start);
            points[1] = ChooseNewRoom(start);
            points[2] = ChooseNewRoom(start);
            points[3] = ChooseNewRoom(start);
            // Initial 4 rooms must be valid, so they will all be set in ChooseNewRoom.
            OrderedPair room;
            while (rooms < totalRooms)
            {
                // validPoints keeps track of spawn points that currently have a (-1, -1) point attached.
                // The corresponding index array value is set to 0 if it is invalid.
                int[] validPoints = { 1, 1, 1, 1 };
                for (int n = 0; n < 4; n++)
                {
                    // Skip over the point if not valid -- i.e. don't try to generate a new room off of (-1, -1).
                    if (validPoints[n] == 0)
                        continue;
                    // If ChooseNewRoom successfully found a room, we can set the corresponding spawn point to start there instead!
                    if ((room = ChooseNewRoom(points[n])).i != -1)
                        points[n] = room;
                    // If ChooseNewRoom found that all adjacent rooms are taken
                    else
                    {
                        // Set the value to invalid.
                        validPoints[n] = 0;
                        // Then, set a new spawn point at a random location.
                        // todo: do something to continue generating from another point
                        /* The following code has a chance to "fail" 
                         * -- i.e. the spawn point was not reset to a valid location this iteration.
                         * In this case, the next while loop iteration has another chance to find a valid location for this spawn point.
                         * Additionally, the other spawn points will still be generating new rooms, as long as they have a valid spawn point!
                         */
                        // This chance gets smaller the more rooms that are added, but it can also be set to a concrete value to increase chance of success.
                        float chance = 1 / AllRooms.Count;
                        foreach (OrderedPair p in AllRooms)
                        {
                            if (Random.Range(0f, 1f) <= chance && (room = ChooseNewRoom(p)).i != -1)
                            {
                                points[n] = room;
                                validPoints[n] = 1;
                            }
                        }

                        // This code actually works, but I want to try something more random!
                        /*for (int m = 0; m < 4; m++)
                        {
                            if (n != m && (room = ChooseNewRoom(points[m])).i != -1)
                            {
                                points[n] = room;
                                break;
                            }
                            dead[n] = 1;
                            Debug.Log("Unable to find a viable room for point " + n + ". Generation  has been terminated.");
                        }*/
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
                int choice = Random.Range(min, max + 1);
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
                        validDirections[0] = 0;
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

                        validDirections[1] = 0;
                        if (min == 2)
                            min++;
                        else if (max == 2)
                            max--;
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
                        if (max == 3)
                            max--;
                        else if (min == 3)
                            min++;
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

                        validDirections[3] = 0;
                        max--;
                        break;
                }
            } while (min <= max && (validDirections[0] == 1 || validDirections[1] == 1 || validDirections[2] == 1 ||
                                    validDirections[3] == 1));

            // Will hit this statement if all rooms are taken!
            AllRooms.Remove(p);
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