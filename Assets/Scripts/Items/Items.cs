using Characters;
using UnityEngine;

namespace Items
{
    public class Items : MonoBehaviour
    {
        protected float hpRestore;
        protected float manaRestore;
        protected float hpIncrease;
        protected float manaIncrease;
        protected float damageIncrease;
        protected float rangeIncrease;
        protected float defenceIncrease;
        protected float magDefenceIncrease;
        protected float speedIncrease;

        public virtual void UpdateCharacterStat(Character player)
        {
            Debug.Log(player.ToString());
            Destroy(gameObject);
        }
    }
}