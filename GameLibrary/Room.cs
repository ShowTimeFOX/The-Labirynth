namespace GameLibrary
{
    public class Room
    {
        public Coordinates Coordinates { get; set; }
        public Wall[] Walls { get; set; }

        public bool HasMonster { get; set; }
        public Monster Monster { get; set; }

        public Room(Coordinates coordinates, Wall[] walls, bool hasMonster = false, Monster monster = null)
        {
            if (walls.Length != 4)
                throw new ArgumentException("There must be exactly 4 walls (North, East, South, West)");
            Coordinates = coordinates;
            Walls = walls;
            HasMonster = hasMonster;
            Monster = monster;
        }
    }
}
