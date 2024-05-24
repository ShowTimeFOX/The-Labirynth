namespace GameLibrary
{
    public class Monster : Character
    {
        public Monster()
        {
        }

        public Monster(string name, string imagePath, int hPCurrent, int hPMax, int strength, int dexterity) : base(name, imagePath, hPCurrent, hPMax, strength, dexterity)
        {
        }
    }
}
