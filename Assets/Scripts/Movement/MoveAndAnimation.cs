using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Movement
{
	public class MoveAndAnimation : MonoBehaviour
	{

		public bool opposite;
		public float speed = 5f;
		private Vector2 targetLocation;

		private Animator animator;
		private string currentState;

		private int direction;

		[SerializeField] private AnimatorOverrideController[] overrideControllers;

		// Start is called before the first frame update
		void Start()
		{
			targetLocation = transform.position;
			animator = GetComponent<Animator>();
		}

		// Update is called once per frame
		void Update()
		{
			if (Input.GetMouseButtonDown(1))
			{
				targetLocation = Camera.main.ScreenToWorldPoint(Input.mousePosition);
				direction = CalculateDirection();
				//Debug.Log(direction==0 ? "up" : direction==1 ? "down" : direction==2 ? "left" : "right");
				SetAnimations(overrideControllers[direction]);
				animator.SetBool("Click", true);
			}

			transform.position = Vector2.MoveTowards(transform.position, opposite ? -1 * targetLocation : targetLocation, speed * Time.deltaTime);

			stopAnimation();
		}

		#region Animation
		public void stopAnimation()
		{
			var currentPos = new Vector2(transform.position.x, transform.position.y);
			if (currentPos == targetLocation)
			{
				animator.SetBool("Click", false);
			}
		}
		public void SetAnimations(AnimatorOverrideController overrideController)
		{
			animator.runtimeAnimatorController = overrideController;
		}

		void changeAnimationState(string newState)
		{
			if (currentState == newState) return;

			animator.Play(newState);

			currentState = newState;
		}
		#endregion Animation

		#region Collision
		void OnCollisionEnter2D(Collision2D coll)
		{
			if (coll.collider == true)
			{
				targetLocation = transform.position;
				animator.SetBool("Click", false);
			}
		}

		void OnCollisionStay2D(Collision2D coll)
		{
			if (coll.collider == true)
			{
				targetLocation = transform.position;
				animator.SetBool("Click", false);
			}
		}
		#endregion Collision

		int CalculateDirection()
		{
			var heading = targetLocation - new Vector2(transform.position.x, transform.position.y);
			var _direction = heading / heading.magnitude;
			float x = _direction.x;
			float y = _direction.y;

			// Horizontal greater = 0, Vertical greater = 1
			int HorV = Math.Max(Math.Abs(y), Math.Abs(x)) == Math.Abs(y) ? 1 : 0;

			return HorV == 1 ? (y > 0 ? 0 : 1) : (x > 0 ? 3 : 2);
		}
	}
}