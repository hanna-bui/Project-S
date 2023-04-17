using Finite_State_Machine.States;
using Finite_State_Machine.States.Abilities;
using UnityEngine;

namespace Characters.Colliders
{
    public class CharacterCollider : RangeCollider
    {
        private void OnTriggerEnter2D(Collider2D col)
        {
            var player = agent as Character;
            if (player == null || !col.gameObject.CompareTag("Enemy") || !player.isSpecialAttack() && player.isBasicAttack()) return;
            
            if (player.isWalkToLocation())
                player.StopAnimation();

            player.Target = col.transform.gameObject;
            player.ChangeState(new BasicAttack());
        }
        private void OnTriggerStay2D(Collider2D col)
        {
            var player = agent as Character;
            if (player == null) return;
            
            if (!col.gameObject.CompareTag("Enemy") || player.isBasicAttack() || player.isWalkToLocation()) return;
            
            if (player.CurrentState is WalkToLocation)
                player.StopAnimation();
            player.Target = col.transform.gameObject;
            player.ChangeState(new BasicAttack());
        }
        
        
    }
}