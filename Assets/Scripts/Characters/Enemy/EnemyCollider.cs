using System;
using Finite_State_Machine.Enemy_States;
using UnityEngine;

namespace Characters.Enemy
{
    public class EnemyCollider : MonoBehaviour
    {
        private Enemy agent;

        private void Start()
        {
            agent = transform.parent.gameObject.GetComponent<Enemy>();
        }
        private void OnTriggerEnter2D(Collider2D col)
        {
            if (col.gameObject.CompareTag("Player"))
            {
                var player = col.gameObject.transform;
                agent.ChangeState(new EnemyAttack(col.transform.gameObject));
            }
        }
        private void OnTriggerExit2D(Collider2D col)
        {
            if (col.gameObject.CompareTag("Player"))
            {
                agent.ChangeState(new PatternWalk());
            }
        }
        public void SetupCollider(float rad)
        {
            var rig = gameObject.AddComponent<Rigidbody2D>();
            rig.bodyType = RigidbodyType2D.Kinematic;
            rig.simulated = true;

            var circle = gameObject.AddComponent<CircleCollider2D>();
            circle.radius = rad/5;
            circle.isTrigger = true;
        }
    }
}