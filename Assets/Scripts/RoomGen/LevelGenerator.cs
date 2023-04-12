using System.Collections;
using System.IO;
using Characters.Enemy;
using Movement.Pathfinding;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;
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
            Item,
            Boss,
            Start,
            End
        }
        
        public enum DoorDir : byte
        {
            E,
            T,
            R,
            TR,
            B,
            I1,
            BR,
            NL,
            L,
            TL,
            I2,
            NB,
            BL,
            NR,
            NT,
            A
        }

        /// <summary>
        /// The Level Matrices.
        /// </summary>
        public RoomType[,] RoomTypes = new RoomType[10, 10];
        public DoorDir[,] RoomDoors = new DoorDir[10, 10];

        [Header("Room Variables")] [Tooltip("How many rooms will be generated.")] [SerializeField]
        private int totalRooms = 40;
        [Tooltip("How many rooms will contain items (consumables excluded).")] [SerializeField]
        private int itemRooms = 5;
        [Tooltip("How many rooms will have a boss fight.")] [SerializeField]
        private int bossRooms = 2;
        [Tooltip("If the boss rooms will be easy (or hard).")] [SerializeField]
        public bool easyBoss = true;

        private int rooms = 0;
        // Size of rooms (all rooms are square so this represents a side).
        // 15 * 15 represents 15 tiles * 15 scale
        private static int s = 15 * 15;
        // Offset from center to place things at
        private Vector3 offsetx = Vector3.right * 15;
        private Vector3 offsety = Vector3.up * 15;
        private Vector3 offsetxy = Vector3.right + Vector3.up;
        // Center of a room to place things at
        private Vector3 center = (Vector3.up + Vector3.right) * (s/2);

        private ArrayList AllRooms = new ArrayList();
        private ArrayList SpawnRooms = new ArrayList();

        private RoomTemplates templates;

        private GameObject Level;

        public NewGrid grid;


        // Start is called before the first frame update
        void Start()
        {
            Level = GameObject.FindGameObjectWithTag("Level");
            templates = GameObject.FindGameObjectWithTag("Rooms").GetComponent<RoomTemplates>();
            int i = 0;
            int j = 0;
            for (; i < 10; i++)
            {
                for (; j < 10; j++)
                {
                    RoomTypes[i, j] = RoomType.Empty;
                    RoomDoors[i, j] = DoorDir.E;
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
            RoomTypes[i, j] = RoomType.Start;
            AllRooms.Add(p);
            SpawnRooms.Add(p);
            // Set camera to Start Room
            Transform camera = GameObject.Find("Main Camera").transform;
            camera.position = ((offsetx * j + offsety * (9-i) + offsetxy * 7.5f) * 15f) + Vector3.back;
            
            // Let's not count the starting room as a room (to make SpawnRooms2 easier to track)
            //rooms++;
            
            //CreateRooms();
            CreateRooms2(p);
            SetDoorDir();
            ChooseRoomTypes();

            CSVWrite("RoomTypes.csv");
            Spawn();
            Debug.Log("Spawned rooms.");
            
            //something to fix Grid
            grid.InitializeGrid();
        }

        /// <summary>
        /// Spawns the Room Template assets in the scene.
        /// </summary>
        private void Spawn()
        {
            //Transform Walkable = GameObject.FindGameObjectWithTag("Walk").transform;
            Vector3 point = Vector3.zero;
            
            for (int j = 0; j <=9; j++)
            {
                for (int i = 9; i >= 0; i--)
                {
                    // d is the door direction enum for the current cell cast to an int
                    int d = (int)RoomDoors[i, j];
                    GameObject room = templates.Rooms[d];
                    room = Instantiate(room, point, room.transform.rotation);
                    room.transform.parent = Level.transform;
                    
                    Tilemap walkable = room.transform.GetChild(1).GetComponent<Tilemap>();
                    Tilemap floor = room.transform.GetChild(3).GetComponent<Tilemap>();
                    
                    grid.UpdateTilemap(walkable, floor, point);

                    // Fill each room.
                    // For now, the spawns are very simple, but can later be updated to be more robust
                    // once we have more items and enemies to choose from!
                    switch (RoomTypes[i, j])
                    {
                        // Normal rooms can just have 1 enemy for now.
                        // todo: Implement random enemy drops on kill (consumables, coins, etc).
                        case RoomType.Normal:
                        {
                            GameObject enemy = templates.Enemies[Random.Range(0, templates.Enemies.Length)];
                            Enemy e = enemy.GetComponent<Enemy>();
                            e.IsBoss = false;
                            e.scale = 0.75f;
                            Instantiate(enemy, point + center, enemy.transform.rotation);
                            break;
                        }
                        /*
                            Item rooms can have 1 item for now.
                            However, this room will have two regular enemies.
                            More complex item and enemy layouts can be implemented later,
                            once complex room layouts with obstacles are implemented.
                        */
                        case RoomType.Item:
                        {
                            GameObject item = templates.Items[Random.Range(0, templates.Items.Length)];
                            item = Instantiate(item, point + center, item.transform.rotation);
                            item.transform.parent = GameObject.Find("Items").transform;
                            item.transform.localScale = offsetx+offsety;
                            GameObject enemy = templates.Enemies[Random.Range(0, templates.Enemies.Length)];
                            Enemy e = enemy.GetComponent<Enemy>();
                            e.IsBoss = false;
                            e.scale = 0.5f;
                            Instantiate(enemy, point + center + offsetx*3, enemy.transform.rotation);
                            Instantiate(enemy, point + center - offsetx*3, enemy.transform.rotation);
                            break;
                        }
                        /*
                            Boss rooms can have 1 giant enemy for now.
                            More complex enemy layouts can be implemented later,
                            once complex room layouts with obstacles are implemented.
                            Note: enemy randomization is commented out for now as we only have 1 enemy type.
                            In the future, this can be either completely random, or semi-random
                            (e.g. 1 Boss enemy, or 2 Medium-difficulty enemies with 2 Easy-difficulty enemies)
                        */
                        case RoomType.Boss:
                        {
                            GameObject enemy = templates.Enemies[Random.Range(0, templates.Enemies.Length)];
                            Enemy e = enemy.GetComponent<Enemy>();
                            e.IsBoss = true;
                            e.scale = 1f;
                            Instantiate(enemy, point + center, enemy.transform.rotation);
                            
                            // Easy Boss means we just spawn the one boss, so we can break here.
                            // Hard Boss will proceed to spawn 4 regular enemies in the corners.
                            if (easyBoss)
                                break;
                            
                            /*
                            // The full code for random enemies below is commented out for now since we only have
                            // 1 enemy type, so repetitive rerolls and variable setting is unnecessary.
                            enemy = templates.Enemies[Random.Range(0, templates.Enemies.Length)];
                            e = enemy.GetComponent<Enemy>();
                            e.IsBoss = false;
                            e.scale = 0.5f;
                            Instantiate(enemy, point + center + offsetxy * 45, enemy.transform.rotation);
                            enemy = templates.Enemies[Random.Range(0, templates.Enemies.Length)];
                            e = enemy.GetComponent<Enemy>();
                            e.IsBoss = false;
                            e.scale = 0.5f;
                            Instantiate(enemy, point + center - offsetxy * 45, enemy.transform.rotation);
                            enemy = templates.Enemies[Random.Range(0, templates.Enemies.Length)];
                            e = enemy.GetComponent<Enemy>();
                            e.IsBoss = false;
                            e.scale = 0.5f;
                            Instantiate(enemy, point + center + (offsetx - offsety) * 3, enemy.transform.rotation);
                            enemy = templates.Enemies[Random.Range(0, templates.Enemies.Length)];
                            e = enemy.GetComponent<Enemy>();
                            e.IsBoss = false;
                            e.scale = 0.5f;
                            Instantiate(enemy, point + center + (offsetx - offsety) * -3, enemy.transform.rotation);
                            */
                            
                            e.IsBoss = false;
                            e.scale = 0.5f;
                            Quaternion eRotation = enemy.transform.rotation;
                            Instantiate(enemy, point + center + offsetxy * 45, eRotation);
                            Instantiate(enemy, point + center - offsetxy * 45, eRotation);
                            Instantiate(enemy, point + center + (offsetx - offsety) * 3, eRotation);
                            Instantiate(enemy, point + center + (offsetx - offsety) * -3, eRotation);
                            
                            break;
                        }
                    }
                    
                    //room.SetActive(false);
                    //walkable.parent = Walkable;
                    
                    //room.transform.localScale = new Vector3(15, 15, 1);
                    point.y += s;
                }
                point.x += s;
                point.y = 0;
            }
            
        }
        
        /// <summary>
        ///  Generator responsible for assigning special room types.
        /// </summary>
        private void ChooseRoomTypes()
        {
            OrderedPair room;
            ArrayList eRooms = new ArrayList();
            foreach (OrderedPair o in AllRooms)
            {
                if (RoomTypes[o.i, o.j] == RoomType.Normal)
                    eRooms.Add(o);
            }

            int rCount = 0;
            for (; rCount < bossRooms; rCount++)
            {
                int r = Random.Range(0, eRooms.Count);
                room = (OrderedPair)eRooms[r];
                RoomTypes[room.i, room.j] = RoomType.Boss;
                eRooms.RemoveAt(r);
            }
            
            for (rCount = 0; rCount < itemRooms; rCount++)
            {
                int r = Random.Range(0, eRooms.Count);
                room = (OrderedPair)eRooms[r];
                RoomTypes[room.i, room.j] = RoomType.Item;
                eRooms.RemoveAt(r);
            }
            
        }
        
        /// <summary>
        ///  Generator responsible for determining the door directions of all rooms.
        /// Super simple code, just increments RoomDoors respectively if an adjacent room is found.
        /// </summary>
        private void SetDoorDir()
        {
            foreach (OrderedPair room in AllRooms)
            { 
                // Room on Top
                if (room.i - 1 >= 0 && RoomTypes[room.i - 1, room.j] != RoomType.Empty)
                    RoomDoors[room.i, room.j] += 1;
                // Room on Right
                if (room.j + 1 <= 9 && RoomTypes[room.i, room.j + 1] != RoomType.Empty)
                    RoomDoors[room.i, room.j] += 2;
                // Room on Bottom
                if (room.i + 1 <= 9 && RoomTypes[room.i + 1, room.j] != RoomType.Empty)
                    RoomDoors[room.i, room.j] += 4;
                // Room on Left
                if (room.j - 1 >= 0 && RoomTypes[room.i, room.j - 1] != RoomType.Empty)
                    RoomDoors[room.i, room.j] += 8;
            }
        }

        /// <summary>
        /// BUG: sometimes, SpawnRooms ends up empty because we remove a room from it every single time.
        /// Generator responsible for creating all the rooms.
        /// It loops until the generated room count (rooms) hits the desired total room count (totalRooms).
        /// Very simple code which results in a more clustered level.
        /// Spawns the exact number of rooms.
        /// </summary>
        private void CreateRooms()
        {
            while (rooms < totalRooms)
            {
                int i = Random.Range(0, SpawnRooms.Count);
                OrderedPair room;
                if((room = ChooseNewRoom((OrderedPair)SpawnRooms[i])).i != -1)
                {
                    SpawnRooms.RemoveAt(i);
                    Debug.Log("Generated room: " + rooms);
                }
            }
        }

        /// <summary>
        /// Generator responsible for creating all the rooms.
        /// It loops until the generated room count (rooms) hits the desired total room count (totalRooms).
        /// More complex code which results in a more spread out level.
        /// Spawns the first multiple of 4 that is >= than totalRooms.
        /// [e.g. with totalRooms = 30, it will spawn 32 rooms]
        /// This method works by keeping 4 "spawn points" which have the potential to spawn a room.
        /// Each iteration, each point tries to choose a new room.
        /// If the room returned is at (-1, -1), then none of the adjacent rooms are valid for spawning a new room.
        /// In this case, the spawn point is considered not valid, and the algorithm will try to set its location
        /// to another randomly-selected room which hopefully has valid adjacent rooms.
        /// </summary>
        private void CreateRooms2(OrderedPair start)
        {
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
                
                for (int n = 0; n < 4; n++)
                {
                    // Break once rooms are all spawned!
                    if (rooms == totalRooms)
                        break;
                    
                    
                    // If ChooseNewRoom successfully found a room, we can set the corresponding spawn point to start there instead!
                    if ((room = ChooseNewRoom(points[n])).i != -1)
                        points[n] = room;
                    
                    // If ChooseNewRoom found that all adjacent rooms are taken
                    else
                    {
                        // Keep trying until the a valid room spawns.
                        do
                        {   
                            // Set spawn point to a random SpawnRooms room - these are all valid.
                            points[n] = (OrderedPair)SpawnRooms[Random.Range(0, SpawnRooms.Count)];
                            /*  This is where things get a bit tricky.
                             *  If a valid room is returned, the following conditions set the spawn point to the new room.
                             *  Then, the do-while exit condition will be met and the spawn point is saved.
                             *  If an invalid room is returned, the exit condition will not be met and the do-while loop
                             *  will execute again - this will overwrite points[n].
                             */
                            room = ChooseNewRoom(points[n]);
                            points[n] = room;
                        } while (points[n].i == -1);
                        /* The following code has a chance to "fail" 
                         * -- i.e. the spawn point was not reset to a valid location this iteration.
                         * In this case, the next while loop iteration has another chance to find a valid location for this spawn point.
                         * Additionally, the other spawn points will still be generating new rooms, as long as they have a valid spawn point!
                         
                        // This chance gets smaller the more rooms that are added, but it can also be set to a concrete value to increase chance of success.
                        float chance = 1 / AllRooms.Count;
                        foreach (OrderedPair p in AllRooms)
                        {
                            if (Random.Range(0f, 1f) <= chance && (room = ChooseNewRoom(p)).i != -1)
                            {
                                points[n] = room;
                                validPoints[n] = 1;
                            }
                        } */
                    }
                    
                    // This will set the last room spawned to be the end
                    if (rooms == totalRooms - 1)
                    {
                        RoomTypes[room.i, room.j] = RoomType.End;
                    }
                }
            }
        }

        /// <summary>
        /// Randomly chooses an Empty room adjacent to the input room.
        /// If the room choice is successful, it updates RoomTypes, AllRooms, and rooms.
        /// Input: OrderedPair p, the input room.
        /// Output: OrderedPair, a room adjacent to start.
        /// </summary>
        private OrderedPair ChooseNewRoom(OrderedPair p)
        {
            // validDirections: init at 1; set to 0 if room is found invalid to avoid redundant checks.
            bool[] validDirections = { true, true, true, true };
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
                        if (validDirections[0] && p.i - 1 >= 0 && RoomTypes[p.i - 1, p.j] == RoomType.Empty)
                        {
                            OrderedPair room = new OrderedPair(p.i - 1, p.j);
                            AddRoom(room);
                            return room;
                        }

                        // if any conditions failed, it is invalid and we can not consider it anymore.
                        validDirections[0] = false;
                        min++;
                        break;
                    case 2: // on right
                        if (validDirections[1] && p.j + 1 <= 9 && RoomTypes[p.i, p.j + 1] == RoomType.Empty)
                        {
                            OrderedPair room = new OrderedPair(p.i, p.j + 1);
                            AddRoom(room);
                            return room;
                        }

                        validDirections[1] = false;
                        if (min == 2)
                            min++;
                        else if (max == 2)
                            max--;
                        break;
                    case 3: // on bottom
                        if (validDirections[2] && p.i + 1 <= 9 && RoomTypes[p.i + 1, p.j] == RoomType.Empty)
                        {
                            OrderedPair room = new OrderedPair(p.i + 1, p.j);
                            AddRoom(room);
                            return room;
                        }

                        validDirections[2] = false;
                        if (max == 3)
                            max--;
                        else if (min == 3)
                            min++;
                        break;
                    case 4: // on left
                        if (validDirections[3] && p.j - 1 >= 0 && RoomTypes[p.i, p.j - 1] == RoomType.Empty)
                        {
                            OrderedPair room = new OrderedPair(p.i, p.j - 1);
                            AddRoom(room);
                            return room;
                        }

                        validDirections[3] = false;
                        max--;
                        break;
                }
            } while (min <= max && (validDirections[0] || validDirections[1] || validDirections[2] || validDirections[3]));

            // Will hit this statement if all rooms are taken!
            SpawnRooms.Remove(p);
            return new OrderedPair(-1, -1);
        }

        /// <summary>
        /// Updates all variables
        /// Adds room to RoomTypes, AllRooms, and SpawnRooms, then increments rooms.
        /// Input: OrderedPair p, the input room.
        /// </summary>
        private void AddRoom(OrderedPair room)
        {
            RoomTypes[room.i, room.j] = RoomType.Normal;
            AllRooms.Add(room);
            SpawnRooms.Add(room);
            rooms++;
        }

        /// <summary>
        /// Write rooms to csv file.
        /// Input: file name for the RoomTypes
        /// </summary>
        /// <param name="filename"></param>
        private void CSVWrite(string filename)
        {
            StreamWriter typewriter = new StreamWriter(filename);
            StreamWriter doorwriter = new StreamWriter("RoomDoors.csv");
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    if (RoomTypes[i, j] == RoomType.Empty)
                        typewriter.Write(",");
                    else
                        typewriter.Write(RoomTypes[i, j] + ",");
                    if (RoomDoors[i, j] == DoorDir.E)
                        doorwriter.Write(",");
                    else
                        doorwriter.Write(RoomDoors[i, j] + ",");
                }
                typewriter.Write(System.Environment.NewLine);
                doorwriter.Write(System.Environment.NewLine);
            }
            typewriter.Flush();
            doorwriter.Flush();
            typewriter.Close();
            doorwriter.Close();
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