using Finite_State_Machine;
using Finite_State_Machine.Enemy_States;
using UnityEngine;

namespace Characters.Colliders
{
    public class EnemyCollider : RangeCollider
    {
        private void OnTriggerEnter2D(Collider2D col)
        {
            if (!col.gameObject.CompareTag("Player")) return;
            var player = col.transform.gameObject;
            agent.ChangeState(new EnemyAttack(player));
        }
        private void OnTriggerExit2D(Collider2D col)
        {
            if (col.gameObject.CompareTag("Player"))
                agent.CurrentState.ToComplete();
        }
    }
}