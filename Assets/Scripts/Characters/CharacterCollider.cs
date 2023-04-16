using Finite_State_Machine;
using Finite_State_Machine.States;
using Finite_State_Machine.States.Abilities;
using Managers;
using UnityEngine;

using Item = Items.Items;

namespace Characters
{
    public class CharacterCollider : MonoBehaviour
    {
        private Character agent;
        private GameManager manager;

        private void Start()
        {
            agent = transform.parent.gameObject.GetComponent<Character>();
            manager = GameManager.instance;
        }
        private void OnTriggerEnter2D(Collider2D col)
        {
            if (col.gameObject.CompareTag("Enemy") && !agent.isAttack())
            {
                if (agent.CurrentState is WalkToLocation)
                    agent.StopAnimation();
                agent.Target = col.transform.gameObject;
                agent.ChangeState(new Attack());
            }
            else if (col.gameObject.name.Contains("Inactive"))
            {
                Debug.Log("Level Complete!");
                manager.Next();
            }
            else if (col.gameObject.name.Contains("Active"))
            {
                Debug.Log("You Won!");
                manager.Win();
            }
        }
        private void OnTriggerStay2D(Collider2D col)
        {
            if (col.gameObject.CompareTag("Enemy") && !agent.isAttack() && !agent.isWalkToLocation())
            {
                if (agent.CurrentState is WalkToLocation)
                    agent.StopAnimation();
                agent.Target = col.transform.gameObject;
                agent.ChangeState(new Attack());
            }
        }
        
        public CircleCollider2D SetupCollider(float rad)
        {
            transform.localScale = new Vector3(0.07f,0.07f,1);
            var rig = gameObject.AddComponent<Rigidbody2D>();
            rig.bodyType = RigidbodyType2D.Kinematic;
            rig.simulated = true;

            var circle = gameObject.AddComponent<CircleCollider2D>();
            circle.radius = rad*5;
            circle.isTrigger = true;

            return circle;
        }
    }
}