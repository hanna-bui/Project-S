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
            {
                player.StateProgress();
                player.StopAnimation();
            }
            
            player.Target = col.gameObject;
            player.select = col.GetComponent<Enemies.Enemy>().select;
            player.select.ToggleOn();
            
            if (player.hasSpecialAttack)
            {
                if (player is Ninja)
                    player.AddState(new ShurikenThrow());
                else {
                    player.AddState(new BasicAttack());
                }
            }
            else
            {
                if (player is Ninja)
                    player.ChangeState(new ShurikenThrow());
                else {
                    player.ChangeState(new BasicAttack());
                }
            }
        }
        private void OnTriggerStay2D(Collider2D col)
        {
            var player = agent as Character;
            if (player == null) return;
            
            if (!col.gameObject.CompareTag("Enemy") || player.isBasicAttack() || player.isWalkToLocation()) return;
            
            if (player.isWalkToLocation())
            {
                player.StateProgress();
                player.StopAnimation();
            }

            player.Target = col.gameObject;
            
            if (player.hasSpecialAttack)
            {
                if (player is Ninja)
                    player.AddState(new ShurikenThrow());
                else {
                    player.AddState(new BasicAttack());
                }
            }
            else
            {
                if (player is Ninja)
                    player.ChangeState(new ShurikenThrow());
                else {
                    player.ChangeState(new BasicAttack());
                }
            }
        }

        private void OnTriggerExit2D(Collider2D col)
        {
            var player = agent as Character;
            if (player.Target == null) return;
            if (player.select!=null)
                player.select.ToggleOff();
            player.Target = null;
        }
    }
}