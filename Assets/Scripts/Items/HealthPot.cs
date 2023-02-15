using Characters;
using UnityEngine;

namespace Items
{
    public class HealthPot : Items
    {
        // Start is called before the first frame update
       private void Start()
       {
           hpIncrease = 5;
       }

       public override void UpdateCharacterStat(Character player)
       {
           player.ChangeHP(hpIncrease);
           base.UpdateCharacterStat(player);
       }
    }
}

