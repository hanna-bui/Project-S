using Characters;

namespace Items
{
    public class Chalice : Items
    {
        // Start is called before the first frame update
        private void Start()
        {
            // temp values -> should restore just be a function to make health = max?
            hpRestore = 10;
            manaRestore = 10;
        }

        public override void UpdateCharacterStat(Character player)
        {
            player.RestoreHealth(hpRestore);
            player.RestoreMana(manaRestore);
            base.UpdateCharacterStat(player);
        }
    }
}