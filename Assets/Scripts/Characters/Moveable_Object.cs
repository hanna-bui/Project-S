using Finite_State_Machine.States;
using System.Collections.Generic;
using Finite_State_Machine;
using UnityEngine;
using System;

using GameManager;

namespace Characters
{

    public class MoveableObject : MonoBehaviour
    {
        public float Speed { get; private set; }

        [SerializeField] protected List<AnimatorOverrideController> overrideControllers;
        public Animator animator;

        public Vector3 TargetLocation { get; set; }
        
        private new Camera camera;
        
        
        #region State

        private State currentState;

        public Manager gm;

        #endregion
        
        
        protected virtual void Start()
        {
            gm = GameObject.Find("GameManager").GetComponent<Manager>();

            camera = Camera.main;
            TargetLocation = transform.position;
            animator = GetComponent<Animator>();

            Speed = 5f;
            currentState = new Idle();
        }

        protected virtual void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                TargetLocation = camera.ScreenToWorldPoint(Input.mousePosition);
                ChangeState(new Walk());
            }

            currentState.Execute(this);
        }
        
        public bool IsAtPosition()
        {
            return transform.position.Equals(TargetLocation);
        }
        
        private int CalculateDirection()
        {
            var position = transform.position;
            if (Math.Abs(TargetLocation.x - position.x) < 0.00001) return 0;
            if (Math.Abs(TargetLocation.y - position.y) < 0.00001) return 0;

            var heading = (Vector2)TargetLocation - (Vector2)position;
            var magnitude = heading / heading.magnitude;
            var x = (decimal)magnitude.x;
            var y = (decimal)magnitude.y;

            var horV = Math.Abs(Math.Max(Math.Abs(y), Math.Abs(x))) == Math.Abs(y) ? 1 : 0;

            return horV == 1 ? y > 0 ? 0 : 1 : x > 0 ? 3 : 2;
        }
        
        #region Animation

        public void StopAnimation()
        {
            animator.SetBool(gm.click, false);
        }

        public void SetAnimations()
        {
            var direction = CalculateDirection();
            var overrideController = overrideControllers[direction];
            animator.runtimeAnimatorController = overrideController;
        }

        #endregion Animation

        #region Getters and Setters

        public void ChangeState(State newState)
        {
            currentState = newState;
        }
        
        public void SetPosition(Vector3 newPosition)
        {
            transform.position = newPosition;
        }

        public Vector3 Position()
        {
            return transform.position;
        }

        #endregion
    }
}