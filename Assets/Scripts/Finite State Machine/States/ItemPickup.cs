using System;
using Characters;
using UnityEngine;
using Item = Items.Items;

namespace Finite_State_Machine.States
{
    public class ItemPickup : State
    {
        // ReSharper disable once InconsistentNaming
        private Item item { get; set; }

        public ItemPickup(Item item)
        {
            this.item = item;
            interval = 0f;
        }

        public override void Execute(MoveableObject agent)
        {
            switch (CurrentStatus)
            {
                case StateStatus.Initialize:
                    break;
                case StateStatus.Executing:
                    item.UpdateCharacterStat(agent as Character);
                    CurrentStatus = StateStatus.Completed;
                    break;
                case StateStatus.Completed:
                    agent.ChangeState(new PlayerIdle());
                    break;
                case StateStatus.Failed:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

    }
}