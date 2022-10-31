using System;
using System.Collections.Generic;
using UnityEngine;

using Grid = Movement.Pathfinding.Grid;

namespace Movement
{
    public class MoveAndAnimation : MonoBehaviour
    {
        [SerializeField] private float speed = 50f;

        [SerializeField] private AnimatorOverrideController[] overrideControllers;

        private Animator animator;
        private string currentState;

        private int direction;
        
        private bool travelOn;
        private bool mouseOn;
        private Vector3 targetLocation;
        private Vector3 mouseLocation;
        private static readonly int Click = Animator.StringToHash("Click");

        private GameObject gridObject;
        private Grid grid;
        private List<Vector3> roadPath;
        
        private new Camera camera;
        

        // Start is called before the first frame update
        private void Start()
        {
            camera = Camera.main;
            targetLocation = transform.position;
            animator = GetComponent<Animator>();
            
            gridObject = GameObject.Find("Grid");
            grid = gridObject.GetComponent<Grid>();
        }

        // Update is called once per frame
        private void Update()
        {
            
            if (Input.GetMouseButtonDown(0))
            {
                mouseLocation = camera.ScreenToWorldPoint(Input.mousePosition);
                roadPath = grid.CreatePath();
                travelOn = true;
                
                direction = CalculateDirection();
                SetAnimations(overrideControllers[direction]);
                animator.SetBool(Click, true);
            }
            
            if (roadPath != null && roadPath.Count!=0)
            {
                targetLocation = roadPath[0];
                
                if (transform.position.Equals(targetLocation)) roadPath.RemoveAt(0);
                
                if (roadPath.Count == 0)
                {
                    roadPath = null;
                    travelOn = false;
                    mouseOn = true;
                }
                direction = CalculateDirection();
                SetAnimations(overrideControllers[direction]);
                transform.position = Vector2.MoveTowards(transform.position, targetLocation, speed * Time.deltaTime);
            }
            else if (!travelOn && mouseOn)
            {
                targetLocation = mouseLocation;
                direction = CalculateDirection();
                SetAnimations(overrideControllers[direction]);
                transform.position = Vector2.MoveTowards(transform.position,mouseLocation, speed * Time.deltaTime);
            }
            StopAnimation();
        }

        private int CalculateDirection()
        {
            if (targetLocation.x == transform.position.x && targetLocation.y == transform.position.y) return 0;
            var heading = (Vector2)targetLocation - (Vector2)transform.position;
            var magnitude = heading / heading.magnitude;
            var x = (decimal)magnitude.x;
            var y = (decimal)magnitude.y;

            // Horizontal greater = 0, Vertical greater = 1
            var horV = Math.Abs(Math.Max(Math.Abs(y), Math.Abs(x))) == Math.Abs(y) ? 1 : 0;

            return horV == 1 ? y > 0 ? 0 : 1 : x > 0 ? 3 : 2;
        }

        #region Animation

        private void StopAnimation()
        {
            if ((Vector2)transform.position == (Vector2)mouseLocation)
            {
                animator.SetBool(Click, false);
                mouseOn = false;
            }
        }

        private void SetAnimations(AnimatorOverrideController overrideController)
        {
            animator.runtimeAnimatorController = overrideController;
        }

        #endregion Animation

        #region Collision

        private void OnCollisionEnter2D(Collision2D coll)
        {
            if (coll.collider == true)
            {
                targetLocation = transform.position;
                animator.SetBool(Click, false);
            }
        }

        private void OnCollisionStay2D(Collision2D coll)
        {
            if (coll.collider == true)
            {
                targetLocation = transform.position;
                animator.SetBool(Click, false);
            }
        }

        #endregion Collision
    }
}