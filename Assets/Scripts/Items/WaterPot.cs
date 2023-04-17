using Characters;

namespace Items
{
    public class WaterPot : Items
    {
        protected override void Start()
        {
            base.Start();
            manaIncrease =  5;
        }

        public override void UpdateCharacterStat(Character player)
        {
            player.ChangeMana(manaIncrease);
            base.UpdateCharacterStat(player);
        }
    }
}