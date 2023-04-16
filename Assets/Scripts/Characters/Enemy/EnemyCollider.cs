using System;
using Finite_State_Machine;
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
                var player = col.transform.gameObject;
                agent.ChangeState(new EnemyAttack(player));
            }
        }
        private void OnTriggerExit2D(Collider2D col)
        {
            if (col.gameObject.CompareTag("Player"))
            {
                agent.CurrentState.ChangeStatus(StateStatus.Completed);
                
            }
        }
        public void SetupCollider(float rad)
        {
            transform.localScale = new Vector3(0.07f,0.07f,1);
            var rig = gameObject.AddComponent<Rigidbody2D>();
            rig.bodyType = RigidbodyType2D.Kinematic;
            rig.simulated = true;

            var circle = gameObject.AddComponent<CircleCollider2D>();
            circle.radius = rad*5;
            circle.isTrigger = true;
        }
    }
}