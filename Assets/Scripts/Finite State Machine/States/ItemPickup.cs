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

        protected override void Initialize(MoveableObject agent) 
        {
             
        }

        protected override void Executing(MoveableObject agent)
        {
            item.UpdateCharacterStat(agent as Character);
            CurrentStatus = StateStatus.Completed;
        }

        protected override void Completed(MoveableObject agent)
        {
            agent.ChangeState(new PlayerIdle());
        }
    }
}