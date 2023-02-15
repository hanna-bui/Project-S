using Characters;

namespace Items
{
    public class WaterPot : Items
    {
        private void Start(){
            manaIncrease =  5;
        }

        public override void UpdateCharacterStat(Character player)
        {
            player.ChangeMana(manaIncrease);
            base.UpdateCharacterStat(player);
        }
    }
}