using Characters;
using UnityEngine;

namespace Items
{
    public class HealthPot : Items
    {
        // Start is called before the first frame update
       protected override void Start()
       {
           base.Start();
           hpIncrease = 5;
       }

       public override void UpdateCharacterStat(Character player)
       {
           player.ChangeHeath(hpIncrease);
           base.UpdateCharacterStat(player);
       }
    }
}

