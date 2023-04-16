using Characters;
using UnityEngine;

namespace Items
{
    public class Items : MonoBehaviour
    {
        protected int hpRestore;
        protected int manaRestore;
        protected int hpIncrease;
        protected int manaIncrease;
        protected int damageIncrease;
        protected int rangeIncrease;
        protected int defenceIncrease;
        protected int magDefenceIncrease;
        protected int speedIncrease;

        public virtual void UpdateCharacterStat(Character player)
        {
            Debug.Log(player.ToString());
            Destroy(gameObject);
        }
    }
}