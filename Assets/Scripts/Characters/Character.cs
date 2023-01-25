using System;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
// using Movement.Steering_Behaviour;
using NewGrid = Movement.Pathfinding.NewGrid;

namespace Characters
{

    [Serializable]
    public class Character : MonoBehaviour
    {
        public String charName;
        public GameObject prefab;
        // All Characters have:
        protected float hp { get; set; } // health points
        protected float chp { get; set; } // current health points
        protected float mp { get; set; } // mana points
        protected float cmp { get; set; } // current mana points
        protected float spe { get; set; } // speed
        protected float ran { get; set; } // range
        protected float dmg { get; set; } // damage
        protected float mdmg { get; set; } // magic damage
        protected float def { get; set; } // defense
        protected float mdef { get; set; } // magic defense

        protected int lvl { get; set; } // level

        // Assets
        protected GameObject sprite { get; set; } // Character Sprite
        protected GameObject weapon { get; set; } // Main Weapon Sprite
        protected GameObject wa1 { get; set; } // A1 Weapon Sprite
        protected GameObject wa2 { get; set; } // A2 Weapon Sprite

        protected GameObject wa3 { get; set; } // A3 Weapon Sprite

        // In enemies, unused abilities can be set to have no effect
        protected void a1()
        {
        } // ability 1

        protected void a2()
        {
        } // ability 2

        protected void a3()
        {
        } // ability 3

        /* Do we need methods like this?
     How are we going to deal damage, heal, etc?
     public float gethp()
    {
        return hp;
    }
    // ALL characters take damage
    // Characters can only be damaged up to hp=0
    public void takeDamage(float dmg)
    {
        hp -= dmg;
        if (hp < 0) hp = 0;
    }
    
    public void inchp(int inc)
    {
        hp += inc;
    }*/

        #region Goals/Movements

        [SerializeField] protected List<AnimatorOverrideController> overrideControllers;
        private Animator animator;

        private Vector2 cPosition;
        private Vector2 cVelocity;
        private Vector2 cHeading;
        private Vector2 cSide;
        protected float cMass { get; set; }

        // protected SteeringBehaviour steering;


        private bool travelOn;
        private bool mouseOn;
        private Vector3 targetLocation;
        private Vector3 mouseLocation;
        private static readonly int Click = Animator.StringToHash("Click");

        private GameObject gridObject;
        private NewGrid grid;
        private List<Vector3> roadPath;

        private new Camera camera;

        protected virtual void Start()
        {
            // steering = new SteeringBehaviour(this);
            // cPosition = transform.position;
            // cVelocity = new Vector2(0, 0);

            camera = Camera.main;
            targetLocation = transform.position;
            animator = GetComponent<Animator>();

            gridObject = GameObject.Find("Grid");
            grid = gridObject.GetComponent<NewGrid>();
        }

        protected virtual void Update()
        {
            // var steeringForce = steering.Calculate();
            // var acceleration = steeringForce / cMass;
            // cVelocity += acceleration * Time.deltaTime;
            // Truncate(spe);
            // SetPosition();
            //
            // if (cVelocity.magnitude > 0.00000001)
            // {
            //     cHeading = cVelocity.normalized;
            //     transform.position = Vector2.MoveTowards(transform.position, cHeading, spe*Time.deltaTime);
            //     cSide = Vector2.Perpendicular(cHeading);
            // }
            //
            // if (Input.GetMouseButtonDown(0))
            // {
            //     var g = new FollowPath(this, camera.ScreenToWorldPoint(Input.mousePosition), grid, overrideControllers);
            //     g.Activate();
            //     g.Process();
            // }

            if (Input.GetMouseButtonDown(0))
            {
                mouseLocation = camera.ScreenToWorldPoint(Input.mousePosition);
                roadPath = grid.CreatePath(transform.position, mouseLocation);
                travelOn = true;

                SetAnimations();
                animator.SetBool(Click, true);
            }

            if (roadPath != null && roadPath.Count != 0)
            {
                targetLocation = roadPath[0];

                if (transform.position.Equals(targetLocation)) roadPath.RemoveAt(0);

                if (roadPath.Count == 0)
                {
                    roadPath = null;
                    travelOn = false;
                    mouseOn = true;
                }
                Move();
            }
            else if (!travelOn && mouseOn)
            {
                targetLocation = mouseLocation;
                Move();
            }

            StopAnimation();
        }

        private int CalculateDirection()
        {
            var position = transform.position;
            if (Math.Abs(targetLocation.x - position.x) < 0.001) return 0;
            if (Math.Abs(targetLocation.y - position.y) < 0.001) return 0;

            var heading = (Vector2)targetLocation - (Vector2)position;
            var magnitude = heading / heading.magnitude;
            var x = (decimal)magnitude.x;
            var y = (decimal)magnitude.y;

            // Horizontal greater = 0, Vertical greater = 1
            var horV = Math.Abs(Math.Max(Math.Abs(y), Math.Abs(x))) == Math.Abs(y) ? 1 : 0;

            return horV == 1 ? y > 0 ? 0 : 1 : x > 0 ? 3 : 2;
        }

        private void Move()
        {
            SetAnimations();
            
            var distance = spe * 10 * Time.deltaTime;
            var towards = Vector2.MoveTowards(transform.position, targetLocation, distance);
            transform.position = towards;
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

        private void SetAnimations()
        {
            var direction = CalculateDirection();
            var overrideController = overrideControllers[direction];
            animator.runtimeAnimatorController = overrideController;
        }

        #endregion Animation

        // public SteeringBehaviour GetSteering()
        // {
        //     return steering;
        // }
        //
        // public void SetPosition()
        // {
        //     cPosition += cVelocity * Time.deltaTime;
        // }
        //
        // public Vector2 GetPosition()
        // {
        //     return this.cPosition;
        // }
        //
        // public Vector2 GetVelocity()
        // {
        //     return this.cVelocity;
        // }
        //
        // private void Truncate(float f)
        // {
        //     cVelocity = Vector3.ClampMagnitude(cVelocity, spe);
        // }
        //
        // public bool IsAtPosition(Vector3 position)
        // {
        //     return transform.position == position;
        // }

        #endregion

    }
}