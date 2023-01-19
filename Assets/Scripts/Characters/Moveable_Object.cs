﻿using System.Collections.Generic;
using Finite_State_Machine;
using UnityEngine;
using GameManager;
using Movement;
using System;

// ReSharper disable InconsistentNaming

namespace Characters
{

    public class MoveableObject : MonoBehaviour
    {
        
        [SerializeField] protected List<AnimatorOverrideController> overrideControllers;
        public Animator animator;

        public Vector3 TargetLocation { get; set; }
        
        protected new Camera camera;
        
        
        #region State

        protected State currentState;

        public Manager gm;
        
        public float Speed { get; private set; }
        
        private Vector3 origin;
        
        // ReSharper disable once ConvertToAutoProperty
        public Vector3 Origin
        {
            get => origin;
            set => origin = value;
        }

        #endregion
        
        
        protected virtual void Start()
        {
            gm = GameObject.Find("GameManager").GetComponent<Manager>();

            camera = Camera.main;
            TargetLocation = transform.position;
            animator = GetComponent<Animator>();

            Speed = 5f;
        }

        protected virtual void Update()
        {
            currentState.Execute(this);
        }
        
        public bool IsAtPosition()
        {
            return transform.position.Equals(TargetLocation);
        }
        
        public bool IsAtPosition(Vector3 target)
        {
            return transform.position.Equals(target);
        }
        
        public int CalculateDirection()
        {
            var position = transform.position;
            if (Math.Abs(TargetLocation.x - position.x) < 0.00001) return 0;
            if (Math.Abs(TargetLocation.y - position.y) < 0.00001) return 0;

            var heading = (Vector2)TargetLocation - (Vector2)position;
            var magnitude = heading / heading.magnitude;
            var x = (decimal)magnitude.x;
            var y = (decimal)magnitude.y;
            
            if (Math.Max(Math.Abs(y), Math.Abs(x)) == Math.Abs(y))
            {
                return y > 0 ? Direction.Up : Direction.Down;
            }
            return x > 0 ? Direction.Right : Direction.Left;
        }
        
        #region Animation

        public void StopAnimation()
        {
            animator.SetBool(gm.click, false);
        }

        public virtual void SetAnimations(int index)
        {
            var overrideController = overrideControllers[index];
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
        
        public Vector3 LocalPosition()
        {
            return transform.localPosition;
        }
        
        public void SetLocalPosition(Vector3 newPosition)
        {
            transform.localPosition = newPosition;
        }

        #endregion
    }
}