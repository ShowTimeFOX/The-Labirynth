namespace GameLibrary
{
    public class Room
    {
        public Coordinates Coordinates { get; set; }
        public Wall WallNorth { get; set; }
        public Wall WallEast { get; set; }
        public Wall WallSouth { get; set; }
        public Wall WallWest { get; set; }

        public bool HasMonster { get; set; }
        public Monster Monster { get; set; }

        public Room(Coordinates coordinates, Wall wallNorth, Wall wallEast, Wall wallSouth, Wall wallWest, bool hasMonster = false, Monster monster = null)
        {
            Coordinates = coordinates;
            WallNorth = wallNorth;
            WallEast = wallEast;
            WallSouth = wallSouth;
            WallWest = wallWest;
            HasMonster = hasMonster;
            Monster = monster;
        }
    }
}
