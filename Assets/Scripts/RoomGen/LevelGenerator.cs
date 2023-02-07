using System.Collections;
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
        public RoomType[,] Level = new RoomType[9,9];

        [Header("Room Variables")] [Tooltip("How many rooms will be generated.")] [SerializeField]
        private int totalRooms = 11;

        private int rooms = 0;


        // Start is called before the first frame update
        void Start()
        {
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    Level[i, j] = RoomType.Empty;
                }
            }

            Level[4, 4] = RoomType.Start;
            Level[3, 4] = RoomType.Normal;
            Level[4, 5] = RoomType.Normal;
            Level[5, 4] = RoomType.Normal;
            Level[4, 3] = RoomType.Normal;
            rooms += 4;
            CreateRooms(new OrderedPair(4, 4));
            for (int i = 0; i < 9; i++)
            {
                Debug.Log((int)Level[i,0]+" "+(int)Level[i,1]+" "+(int)Level[i,2]+" "+(int)Level[i,3]+" "+(int)Level[i,4]+" "+(int)Level[i,5]+" "+(int)Level[i,6]+" "+(int)Level[i,7]+" "+(int)Level[i,8]);
                /*
                for (int j = 0; j < 9; j++)
                {
                    Debug.Log("Level["+i+","+j+"] = "+Level[i,j]);
                }*/
            }
            
        }

        private void CreateRooms(OrderedPair p)
        {
            int i = 1;
            while (rooms < totalRooms)
            {
                OrderedPair room;
                room = ChooseRoom(new OrderedPair(p.i - i, p.j));
                Level[room.i, room.j] = RoomType.Normal;
                room = ChooseRoom(new OrderedPair(p.i, p.j + i));
                Level[room.i, room.j] = RoomType.Normal;
                room = ChooseRoom(new OrderedPair(p.i + i, p.j));
                Level[room.i, room.j] = RoomType.Normal;
                room = ChooseRoom(new OrderedPair(p.i, p.j - i));
                Level[room.i, room.j] = RoomType.Normal;
                rooms += 4;
                i++;
            }
        }

        private OrderedPair ChooseRoom(OrderedPair p)
        {
            bool chosen = false;
            do
            {
                int choice = Random.Range(1, 4);
                switch (choice)
                {
                    case 1: // on top
                        if (p.i - 1 >= 0 && Level[p.i - 1, p.j] == RoomType.Empty)
                        {
                            chosen = true;
                            return new OrderedPair(p.i - 1, p.j);
                        }

                        break;
                    case 2: // on right
                        if (p.j + 1 <= 8 && Level[p.i, p.j + 1] == RoomType.Empty)
                        {
                            chosen = true;
                            return new OrderedPair(p.i - 1, p.j);
                        }

                        break;
                    case 3: // on bottom
                        if (p.i + 1 <= 8 && Level[p.i + 1, p.j] == RoomType.Empty)
                        {
                            chosen = true;
                            return new OrderedPair(p.i - 1, p.j);
                        }

                        break;
                    case 4: // on left
                        if (p.j - 1 >= 0 && Level[p.i, p.j - 1] == RoomType.Empty)
                        {
                            chosen = true;
                            return new OrderedPair(p.i - 1, p.j);
                        }

                        break;
                }
            } while (!chosen);

            // Should never hit this statement
            return new OrderedPair(-1, -1);
        }
    }

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