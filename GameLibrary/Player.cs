namespace GameLibrary
{
    public class Player : Character
    {
        public Coordinates Coordinates { get; set; }
        public EDirection Direction { get; set; }

        public Player()
        {
        }

        public Player(string name, string imagePath, int hPCurrent, int hPMax, int strength, int dexterity) : base(name, imagePath, hPCurrent, hPMax, strength, dexterity)
        {
        }


        
    }
}
