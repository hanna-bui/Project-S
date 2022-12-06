using System;
using System.Collections.Generic;
using UnityEngine;

using NewGrid = Movement.Pathfinding.NewGrid;

namespace Movement
{
    public class EnemyMovement : MonoBehaviour
    {
        public enum MovementOptions
        {
            Plus,
            Side,
            Random,
            None
        };

        public MovementOptions movementStyle = MovementOptions.Plus;
        private Vector3 origin;
        private int scale = 20;
        
        [SerializeField] private float speed = 10f;
        
        private GameObject gridObject;
        private NewGrid grid;
        private List<Vector3> roadPath;

        private List<Vector3> plus;
        private int index;
        
        private List<Vector3> side;
    
        private List<Vector3> random;
        private Vector3 newPos;

        private void Start()
        {
            origin = transform.localPosition;
            gridObject = GameObject.Find("Grid");
            grid = gridObject.GetComponent<NewGrid>();

            plus = new List<Vector3>();
            side = new List<Vector3>();
            random = null;
            
            SetupPlus();
            
            SetupSide();
        }

        // Update is called once per frame
        private void Update()
        {
            switch (movementStyle)
            {
                case MovementOptions.Plus:
                    PlusMovement();
                    break;
                case MovementOptions.Side:
                    SideMovement();
                    break;
                case MovementOptions.Random:
                    RandomMovement();
                    break;
                case MovementOptions.None:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void SetupPlus()
        {
            plus.Add(origin);
            
            var y = 20f;
            while (!grid.IsWalkable(transform.position + new Vector3(0, y, 0)) && y > 0)
            {
                y -= 1f;
            }
            plus.Add(origin + new Vector3(0, y/scale, 0));
            
            plus.Add(origin);
            
            var x = 20f;
            while (!grid.IsWalkable(transform.position + new Vector3(x, 0, 0)) && x > 0)
            {
                x -= 1f;
            }
            plus.Add(origin + new Vector3(x/scale, 0, 0));
            
            plus.Add(origin);
            
            y = -20f;
            while (!grid.IsWalkable(transform.position + new Vector3(0, y, 0)) && y < 0)
            {
                y += 1f;
            }
            plus.Add(origin + new Vector3(0, y/scale, 0));
            
            plus.Add(origin);
            
            x = -20f;
            while (!grid.IsWalkable(transform.position + new Vector3(x, 0, 0)) && x < 0)
            {
                x += 1f;
            }
            plus.Add(origin + new Vector3(x/scale, 0, 0));
        }

        private void PlusMovement()
        {
            var newPosition = plus[index];
            if (transform.localPosition != plus[index])
            {
                transform.localPosition =
                    Vector3.MoveTowards(transform.localPosition, newPosition, speed/50 * Time.deltaTime);
            }
            else
            {
                index++;
                if (index == plus.Count)
                {
                    index = 0;
                }
            }
        }

        private void SetupSide()
        {
            side.Add(origin);
            var x = 20f;
            while (!grid.IsWalkable(transform.position + new Vector3(x, 0, 0)) && x > 0)
            {
                x -= 1f;
            }
            side.Add(origin + new Vector3(x/scale, 0, 0));
            side.Add(origin);
            x = -20f;
            while (!grid.IsWalkable(transform.position + new Vector3(x, 0, 0)) && x < 0)
            {
                x += 1f;
            }
            side.Add(origin + new Vector3(x/scale, 0, 0));
        }

        private void SideMovement()
        {
            var newPosition = side[index];
            if (transform.localPosition != side[index])
            {
                transform.localPosition =
                    Vector3.MoveTowards(transform.localPosition, newPosition, speed/50 * Time.deltaTime);
            }
            else
            {
                index++;
                if (index == side.Count)
                {
                    index = 0;
                }
            }
        }

        private void RandomMovement()
        {
            if (random==null)
            {
                newPos = grid.GetRandomCoords();
                random = grid.CreatePath(transform, newPos);
            }

            if (random != null)
            {
                var targetLocation = random[0];
                
                if (transform.position.Equals(targetLocation)) random.RemoveAt(0);
                
                if (random.Count == 0)
                {
                    random = null;
                }
                transform.position = Vector3.MoveTowards(transform.position, targetLocation, speed * Time.deltaTime);
            }
        }
    }
}