using Finite_State_Machine;
using Finite_State_Machine.Enemy_States;
using Finite_State_Machine.States;
using UnityEngine;

namespace Characters.Colliders
{
    public class AgroCollider : RangeCollider
    {
        private Vector3 target;
        
        private void OnTriggerEnter2D(Collider2D col)
        {
            var enemy = agent as Enemies.Enemy;
            if (enemy == null) return;
            
            if (!col.gameObject.CompareTag("Player")) return;
            target = col.transform.position;
            enemy.TargetLocation = target;
            agent.ChangeState(new WalkToLocation());
        }
        
        private void OnTriggerStay2D(Collider2D col)
        {
            var enemy = agent as Enemies.Enemy;
            if (enemy == null) return;
            
            if (!col.gameObject.CompareTag("Player") || enemy.IsBasicAttack()) return;
            if (!enemy.IsWalkToLocation())
            {
                if (!target.Equals(col.transform.position))
                {
                    target = col.transform.position;
                    enemy.TargetLocation = target;
                }
                agent.ChangeState(new WalkToLocation());
            }
            else
            {
                if (target.Equals(col.transform.position)) return;
                target = col.transform.position;
                enemy.TargetLocation = target;
                agent.ChangeState(new WalkToLocation());
            }
        }
        
        private void OnTriggerExit2D(Collider2D col)
        {
            if (col.gameObject.CompareTag("Player"))
            {
                agent.CurrentState.ToComplete();
            }
        }
    }
}