using Characters;

namespace Items
{
    public class MedKit : Items
    {
        private void Start()
        {
            hpRestore = 1000;
            manaRestore = 1000;
        }

        public override void UpdateCharacterStat(Character player)
        {
            player.RestoreHP(hpRestore);
            player.RestoreMana(manaRestore);
            base.UpdateCharacterStat(player);
        }
    }
}