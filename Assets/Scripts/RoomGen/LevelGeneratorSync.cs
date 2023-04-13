// using System.Collections;
// using System.Collections.Generic;
// using System.Linq;
// using Characters.Enemy;
// using Mirror;
// using Movement.Pathfinding;
// using UnityEngine;
// using UnityEngine.Tilemaps;
// using RT = RoomGen.LevelGenerator.RoomType;
//
// // using DD = RoomGen.LevelGenerator.DoorDir;
// // ReSharper disable ParameterHidesMember
// // ReSharper disable UnusedParameter.Local
// // ReSharper disable FieldCanBeMadeReadOnly.Local
//
// namespace RoomGen
// {
//     public enum DD : int
//     {
//         E,
//         T,
//         R,
//         TR,
//         B,
//         I1,
//         BR,
//         NL,
//         L,
//         TL,
//         I2,
//         NB,
//         BL,
//         NR,
//         NT,
//         A
//     }
//
//     public class LevelGeneratorSync : NetworkBehaviour
//     {
//         private static Hashtable _combo = new Hashtable()
//         {
//             { "T", new List<DD>() { DD.B, DD.BL, DD.BR, DD.I1, DD.NL, DD.NR, DD.NT } },
//             { "B", new List<DD>() { DD.A, DD.I1, DD.NB, DD.NL, DD.NR, DD.T, DD.TL, DD.TR } },
//             { "L", new List<DD>() { DD.A, DD.BR, DD.I2, DD.NB, DD.NT, DD.R, DD.TR } },
//             { "R", new List<DD>() { DD.A, DD.BL, DD.I2, DD.L, DD.NB, DD.NR, DD.NT, DD.TL } },
//             { "E", new List<DD>() { DD.E } }
//         };
//
//         private static Hashtable _combo2 = new Hashtable()
//         {
//             {
//                 DD.E, new Hashtable()
//                 {
//                     { "T", _combo["E"] },
//                     { "B", _combo["E"] },
//                     { "L", _combo["E"] },
//                     { "R", _combo["E"] }
//                 }
//             },
//             {
//                 DD.T, new Hashtable()
//                 {
//                     { "T", _combo["T"] },
//                     { "B", _combo["E"] },
//                     { "L", _combo["E"] },
//                     { "R", _combo["E"] }
//                 }
//             },
//             {
//                 DD.R, new Hashtable()
//                 {
//                     { "T", _combo["E"] },
//                     { "B", _combo["E"] },
//                     { "L", _combo["E"] },
//                     { "R", _combo["R"] }
//                 }
//             },
//             {
//                 DD.TR, new Hashtable()
//                 {
//                     { "T", _combo["T"] },
//                     { "B", _combo["E"] },
//                     { "L", _combo["E"] },
//                     { "R", _combo["R"] }
//                 }
//             },
//             {
//                 DD.B, new Hashtable()
//                 {
//                     { "T", _combo["E"] },
//                     { "B", _combo["B"] },
//                     { "L", _combo["E"] },
//                     { "R", _combo["E"] }
//                 }
//             },
//             {
//                 DD.I1, new Hashtable()
//                 {
//                     { "T", _combo["E"] },
//                     { "B", _combo["E"] },
//                     { "L", _combo["L"] },
//                     { "R", _combo["R"] }
//                 }
//             },
//             {
//                 DD.BR, new Hashtable()
//                 {
//                     { "T", _combo["E"] },
//                     { "B", _combo["B"] },
//                     { "L", _combo["E"] },
//                     { "R", _combo["R"] }
//                 }
//             },
//             {
//                 DD.NL, new Hashtable()
//                 {
//                     { "T", _combo["T"] },
//                     { "B", _combo["B"] },
//                     { "L", _combo["E"] },
//                     { "R", _combo["R"] }
//                 }
//             },
//             {
//                 DD.L, new Hashtable()
//                 {
//                     { "T", _combo["E"] },
//                     { "B", _combo["E"] },
//                     { "L", _combo["L"] },
//                     { "R", _combo["E"] }
//                 }
//             },
//             {
//                 DD.TL, new Hashtable()
//                 {
//                     { "T", _combo["T"] },
//                     { "B", _combo["E"] },
//                     { "L", _combo["L"] },
//                     { "R", _combo["E"] }
//                 }
//             },
//             {
//                 DD.I2, new Hashtable()
//                 {
//                     { "T", _combo["E"] },
//                     { "B", _combo["E"] },
//                     { "L", _combo["L"] },
//                     { "R", _combo["R"] }
//                 }
//             },
//             {
//                 DD.NB, new Hashtable()
//                 {
//                     { "T", _combo["T"] },
//                     { "B", _combo["E"] },
//                     { "L", _combo["L"] },
//                     { "R", _combo["R"] }
//                 }
//             },
//             {
//                 DD.BL, new Hashtable()
//                 {
//                     { "T", _combo["E"] },
//                     { "B", _combo["B"] },
//                     { "L", _combo["L"] },
//                     { "R", _combo["E"] }
//                 }
//             },
//             {
//                 DD.NR, new Hashtable()
//                 {
//                     { "T", _combo["T"] },
//                     { "B", _combo["B"] },
//                     { "L", _combo["L"] },
//                     { "R", _combo["E"] }
//                 }
//             },
//             {
//                 DD.NT, new Hashtable()
//                 {
//                     { "T", _combo["E"] },
//                     { "B", _combo["B"] },
//                     { "L", _combo["L"] },
//                     { "R", _combo["R"] }
//                 }
//             },
//             {
//                 DD.A, new Hashtable()
//                 {
//                     { "T", _combo["T"] },
//                     { "B", _combo["B"] },
//                     { "L", _combo["L"] },
//                     { "R", _combo["R"] }
//                 }
//             }
//         };
//
//         private int startI = 4;
//         private int startJ = 4;
//         private int lvlScale = 3;
//         private int totalRooms = 30;
//         private int itemRooms = 9;
//         private int bossRooms = 3;
//         private int seed = 12345;
//         private int rooms;
//
//         public GameObject[] templates;
//         public GameObject[] enemies;
//         public GameObject[] consumables;
//         public GameObject[] items;
//         private NewGrid grid;
//
//         private List<Room> allRooms = new List<Room>();
//         private List<OrderedPair> spawnRooms = new List<OrderedPair>();
//
//         private List<Room> r = Enumerable.Range(0, 10)
//             .SelectMany(i => Enumerable.Range(0, 10)
//                 .Select(j => new Room(new OrderedPair(i, j), RT.Empty, DD.E)))
//             .ToList();
// // [SyncVar(hook = nameof(OnRoomChanged))]
// // private List<Room> r = Enumerable.Range(0, 10)
// //     .SelectMany(i => Enumerable.Range(0, 10)
// //         .Select(j => new Room(new OrderedPair(i, j), RT.Empty, DD.E)))
// //     .ToList();
//
// // private void OnIChanged(int oldValue, int newValue)
// // {
// //     start_i = newValue;
// // }
// //
// // private void OnJChanged(int oldValue, int newValue)
// // {
// //     start_j = newValue;
// // }
//
//         private void OnScaleChanged(int oldValue, int newValue)
//         {
//             lvlScale = newValue;
//             totalRooms = lvlScale * 10;
//             itemRooms = lvlScale * 3;
//             bossRooms = lvlScale;
//         }
//
//         private void OnTotalChanged(int oldValue, int newValue)
//         {
//             totalRooms = newValue;
//         }
//
//         private void OnItemChanged(int oldValue, int newValue)
//         {
//             itemRooms = newValue;
//         }
//
//         private void OnBossChanged(int oldValue, int newValue)
//         {
//             bossRooms = newValue;
//         }
//
//         private void OnRoomChanged(List<Room> oldValue, List<Room> newValue)
//         {
//             r = newValue;
//         }
//
//         private void OnAllRoomChanged(List<Room> oldValue, List<Room> newValue)
//         {
//             allRooms = newValue;
//         }
//
//         private void OnSpawnRoomChanged(List<OrderedPair> oldValue, List<OrderedPair> newValue)
//         {
//             spawnRooms = newValue;
//         }
//
//
//         void Awake()
//         {
//             DontDestroyOnLoad(gameObject);
//             // seed = Random.Range(0, 100000);
//         }
//
//         private void Start()
//         {
//             grid = GetComponent<NewGrid>();
//
//             GenerateLevel(seed);
//         }
//
//         private void OnLevelGenerated(int oldValue, int newValue)
//         {
//             seed = new System.Random(newValue).Next();
//             Random.InitState(seed);
//
//             if (isServer)
//             {
//                 CmdSyncSeed(seed);
//             }
//         }
//
//         private void GenerateLevel(int seed)
//         {
//             // if (!NetworkServer.active) 
//             // {
//             //     Debug.LogError("Cannot generate level when NetworkServer is not active.");
//             //     return;
//             // }
//             
//             // Generate the level based on the seed
//             // This code should be identical on both the server and client
//             // Make sure to synchronize any random number generators
//             // and any other state necessary for generating the level
//
//             var p = new OrderedPair(startI, startJ);
//
//             ChangeRT(r, new OrderedPair(startI, startJ), RT.Start);
//             spawnRooms.Add(p);
//             OnSpawnRoomChanged(spawnRooms, spawnRooms.ToList());
//             allRooms.Add(new Room(p, RT.Start, DD.E));
//             OnAllRoomChanged(allRooms, allRooms.ToList());
//
//             RoomCreator(r, p);
//             SetDoorDir(r);
//             ChooseRoomTypes(r);
//
//             var spawnedObjects = Spawn(r).ToList();
//             
//             foreach (GameObject obj in spawnedObjects)
//             {
//                 NetworkServer.Spawn(obj);
//             }
//         }
//
//         [Command]
//         private void CmdSyncSeed(int newSeed)
//         {
//             seed = newSeed;
//         }
//
//         public override void OnStartServer()
//         {
//             grid = GetComponent<NewGrid>();
//             Random.InitState(seed);
//             RpcSyncSeed(seed);
//         }
//
//         [ClientRpc]
//         private void RpcSyncSeed(int seed)
//         {
//             this.seed = seed;
//         }
//
//         public override void OnStartClient()
//         {
//             base.OnStartClient();
//             OnLevelGenerated(seed, seed);
//         }
//
//         #region Level Generation
//
//         private void ChangeRT(List<Room> r, OrderedPair p, RT roomType)
//         {
//             var temp = r.ToList();
//             var room = Find(temp, p);
//             room.rt = roomType;
//             OnRoomChanged(r, temp.ToList());
//         }
//
//         private void ChangeDD(List<Room> r, OrderedPair p, DD doorDir)
//         {
//             var temp = r.ToList();
//             var room = Find(temp, p);
//             room.dd = doorDir;
//             OnRoomChanged(r, temp.ToList());
//         }
//
//         private bool isRT(List<Room> r, OrderedPair p, RT roomType)
//         {
//             var room = Find(r, p);
//             return room.rt == roomType;
//         }
//
//         private bool isDD(List<Room> r, OrderedPair p, DD doorDir)
//         {
//             var room = Find(r, p);
//             return room.dd == doorDir;
//         }
//
//         private static Room Find(List<Room> r, int i, int j)
//         {
//             foreach (var t in r)
//             {
//                 if (t.i == i && t.j == j)
//                     return t;
//             }
//
//             return null;
//         }
//
//         private static Room Find(List<Room> r, OrderedPair p)
//         {
//             foreach (var t in r)
//             {
//                 if (t.i == p.i && t.j == p.j)
//                     return t;
//             }
//
//             return null;
//         }
//
//         private void AddRoom(List<Room> r, OrderedPair p)
//         {
//             ChangeRT(r, p, RT.Normal);
//             allRooms.Add(new Room(p, RT.Normal, DD.E));
//             OnAllRoomChanged(allRooms, allRooms.ToList());
//             spawnRooms.Add(p);
//             OnSpawnRoomChanged(spawnRooms, spawnRooms.ToList());
//             rooms++;
//         }
//
//         private void ChooseRoomTypes(List<Room> r)
//         {
//             OrderedPair room;
//             var eRooms = new List<OrderedPair>();
//             foreach (var t in r)
//             {
//                 if (Find(r, t.i, t.j).rt == RT.Normal)
//                     eRooms.Add(t.pair);
//             }
//
//             int rCount;
//             for (rCount = 0; rCount < bossRooms; rCount++)
//             {
//                 var s = Random.Range(0, eRooms.Count);
//                 room = eRooms[s];
//                 Find(r, room.i, room.j).rt = RT.Boss;
//                 eRooms.RemoveAt(s);
//             }
//
//             for (rCount = 0; rCount < itemRooms; rCount++)
//             {
//                 var s = Random.Range(0, eRooms.Count);
//                 room = eRooms[s];
//                 Find(r, room.i, room.j).rt = RT.Item;
//                 eRooms.RemoveAt(s);
//             }
//         }
//
//         private void SetDoorDir(List<Room> r)
//         {
//             foreach (var t in allRooms)
//             {
//                 var x = t.i;
//                 var y = t.j;
//                 var room = Find(r, t.pair);
//
//                 // Check for neighboring rooms and update the dd value accordingly
//                 if (x - 1 >= 0 && Find(r, x - 1, y).rt != RT.Empty)
//                 {
//                     ChangeDD(r, t.pair, room.dd + 1);
//                 } // Room on Top
//
//                 if (y + 1 <= 9 && Find(r, x, y + 1).rt != RT.Empty)
//                 {
//                     ChangeDD(r, t.pair, room.dd + 2);
//                 } // Room on Right
//
//                 if (x + 1 <= 9 && Find(r, x + 1, y).rt != RT.Empty)
//                 {
//                     ChangeDD(r, t.pair, room.dd + 4);
//                 } // Room on Bottom
//
//                 if (y - 1 >= 0 && Find(r, x, y - 1).rt != RT.Empty)
//                 {
//                     ChangeDD(r, t.pair, room.dd + 8);
//                 } // Room on Left
//             }
//         }
//
//         private void RoomCreator(List<Room> r, OrderedPair start)
//         {
//             var points = new OrderedPair[4];
//             points[0] = ChooseNewRoom(r, start);
//             points[1] = ChooseNewRoom(r, start);
//             points[2] = ChooseNewRoom(r, start);
//             points[3] = ChooseNewRoom(r, start);
//             while (rooms <= totalRooms)
//             {
//                 for (var n = 0; n < 4; n++)
//                 {
//                     if (rooms == totalRooms + 1)
//                         break;
//
//                     OrderedPair pair;
//                     if ((pair = ChooseNewRoom(r, points[n])).i != -1)
//                         points[n] = pair;
//                     else
//                     {
//                         do
//                         {
//                             var p = spawnRooms[Random.Range(0, spawnRooms.Count)];
//                             pair = ChooseNewRoom(r, p);
//                             points[n] = pair;
//                         } while (points[n].i == -1);
//                     }
//
//                     // This will set the last room spawned to be the end
//                     if (rooms == totalRooms - 1)
//                     {
//                         ChangeRT(r, pair, RT.End);
//                     }
//                 }
//             }
//         }
//
//         private OrderedPair ChooseNewRoom(List<Room> r, OrderedPair p)
//         {
//             bool[] validDirections = { true, true, true, true };
//             var min = 1;
//             var max = 4;
//             do
//             {
//                 var choice = Random.Range(min, max + 1);
//                 OrderedPair pair;
//                 switch (choice)
//                 {
//                     case 1: // on top
//                         pair = new OrderedPair(p.i - 1, p.j);
//                         if (validDirections[0] && pair.i >= 0 && isRT(r, pair, RT.Empty))
//                         {
//                             AddRoom(r, pair);
//                             return pair;
//                         }
//
//                         validDirections[0] = false;
//                         min++;
//                         break;
//                     case 2: // on right
//                         pair = new OrderedPair(p.i, p.j + 1);
//                         if (validDirections[1] && pair.j <= 9 && isRT(r, pair, RT.Empty))
//                         {
//                             AddRoom(r, pair);
//                             return pair;
//                         }
//
//                         validDirections[1] = false;
//                         if (min == 2)
//                             min++;
//                         else if (max == 2)
//                             max--;
//                         break;
//                     case 3: // on bottom
//                         pair = new OrderedPair(p.i + 1, p.j);
//                         if (validDirections[2] && pair.i <= 9 && isRT(r, pair, RT.Empty))
//                         {
//                             AddRoom(r, pair);
//                             return pair;
//                         }
//
//                         validDirections[2] = false;
//                         if (max == 3)
//                             max--;
//                         else if (min == 3)
//                             min++;
//                         break;
//                     case 4: // on left
//                         pair = new OrderedPair(p.i, p.j - 1);
//                         if (validDirections[3] && pair.j >= 0 && isRT(r, pair, RT.Empty))
//                         {
//                             AddRoom(r, pair);
//                             return pair;
//                         }
//
//                         validDirections[3] = false;
//                         max--;
//                         break;
//                 }
//             } while (min <= max &&
//                      (validDirections[0] || validDirections[1] || validDirections[2] || validDirections[3]));
//
//             // Will hit this statement if all rooms are taken!
//             spawnRooms.Remove(p);
//             OnSpawnRoomChanged(spawnRooms, spawnRooms.ToList());
//
//             return new OrderedPair(-1, -1);
//         }
//
//         private List<GameObject> Spawn(List<Room> r)
//         {
//             var spawnedObjects = new List<GameObject>();
//             
//             var point = Vector3.zero;
//
//             const int size = 15 * 15;
//
//             var center = (Vector3.up + Vector3.right) * (size / 2);
//
//             bool easyBoss = true;
//
//             var offsetx = Vector3.right * 15;
//             var offsety = Vector3.up * 15;
//             var offsetxy = Vector3.right + Vector3.up;
//
//             for (var j = 0; j <= 9; j++)
//             {
//                 for (var i = 9; i >= 0; i--)
//                 {
//                     var t = Find(r, i, j);
//                     t.g = templates[(int)t.dd];
//                     var room = Instantiate(t.g, point, t.g.transform.rotation);
//                     
//                     spawnedObjects.Add(room);
//
//                     room.transform.SetParent(transform);
//
//                     var walkable = room.transform.GetChild(1).GetComponent<Tilemap>();
//                     var floor = room.transform.GetChild(3).GetComponent<Tilemap>();
//
//                     grid.UpdateTilemap(walkable, floor, point, 0);
//
//                     switch (t.rt)
//                     {
//                         case RT.Normal:
//                         {
//                             var enemy = enemies[Random.Range(0, enemies.Length)];
//                             var e = enemy.GetComponent<Enemy>();
//                             e.IsBoss = false;
//                             e.scale = 0.75f;
//                             var temp = Instantiate(enemy, point + center, enemy.transform.rotation);
//                             spawnedObjects.Add(temp);
//                             break;
//                         }
//                         case RT.Item:
//                         {
//                             var item = consumables[Random.Range(0, consumables.Length)];
//                             item = Instantiate(item, point + center, item.transform.rotation);
//                             item.transform.parent = GameObject.Find("Items").transform;
//                             item.transform.localScale = offsetx + offsety;
//                             spawnedObjects.Add(item);
//                             var enemy = enemies[Random.Range(0, enemies.Length)];
//                             var e = enemy.GetComponent<Enemy>();
//                             e.IsBoss = false;
//                             e.scale = 0.5f;
//                             var temp = Instantiate(enemy, point + center + offsetx * 3, enemy.transform.rotation);
//                             spawnedObjects.Add(temp);
//                             temp = Instantiate(enemy, point + center - offsetx * 3, enemy.transform.rotation);
//                             spawnedObjects.Add(temp);
//                             break;
//                         }
//                         case RT.Boss:
//                         {
//                             var enemy = enemies[Random.Range(0, enemies.Length)];
//                             var e = enemy.GetComponent<Enemy>();
//                             e.IsBoss = true;
//                             e.scale = 1f;
//                             var temp = Instantiate(enemy, point + center, enemy.transform.rotation);
//                             spawnedObjects.Add(temp);
//
//                             // Easy Boss means we just spawn the one boss, so we can break here.
//                             // Hard Boss will proceed to spawn 4 regular enemies in the corners.
//                             if (easyBoss)
//                                 break;
//
//                             e.IsBoss = false;
//                             e.scale = 0.5f;
//                             Quaternion eRotation = enemy.transform.rotation;
//                             temp = Instantiate(enemy, point + center + offsetxy * 45, eRotation);
//                             spawnedObjects.Add(temp);
//                             temp = Instantiate(enemy, point + center - offsetxy * 45, eRotation);
//                             spawnedObjects.Add(temp);
//                             temp = Instantiate(enemy, point + center + (offsetx - offsety) * 3, eRotation);
//                             spawnedObjects.Add(temp);
//                             temp = Instantiate(enemy, point + center + (offsetx - offsety) * -3, eRotation);
//                             spawnedObjects.Add(temp);
//
//                             break;
//                         }
//                         case RT.Empty:
//                         case RT.Start:
//                         case RT.End:
//                         default:
//                             break;
//                     }
//
//                     point.y += size;
//                 }
//
//                 point.x += size;
//                 point.y = 0;
//             }
//
//             //something to fix Grid
//             grid.InitializeGrid();
//
//             return spawnedObjects;
//         }
//
//         #endregion
//     }
// }