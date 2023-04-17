using Characters;

namespace Items
{
    public class MedKit : Items
    {
        protected override void Start()
        {
            base.Start();
            hpRestore = 1000;
            manaRestore = 1000;
        }

        public override void UpdateCharacterStat(Character player)
        {
            player.RestoreHealth(hpRestore);
            player.RestoreMana(manaRestore);
            base.UpdateCharacterStat(player);
        }
    }
}