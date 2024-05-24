using static System.Net.Mime.MediaTypeNames;

namespace GameLibrary
{
    public class Wall
    {
        public String ImagePath { get; set; } //to jesli chcemy zeby sciany byly rozne
        public EWallDirection Direction { get; set; }
        public EWallType WallType { get; set; }
        List<Item> Items = new List<Item>();
        public string ImageDirectory { get; set; }

        public Wall(EWallDirection wallDirection, EWallType wallType)
        {
            ImageDirectory = Path.Combine("..", "..", "..", "..", "img/");
            WallType = wallType;
            Direction = wallDirection;
            // Ustawianie ścieżki do grafiki na podstawie typu ściany i kierunku
            switch (wallType)
            {
                case EWallType.Solid:
                    ImagePath = GetSolidWallImagePath(wallDirection);
                    break;
                case EWallType.Empty:
                    ImagePath = GetEmptyWallImagePath(wallDirection);
                    break;
                case EWallType.Door:
                    ImagePath = GetDoorImagePath(wallDirection);
                    break;
                default:
                    ImagePath = "";
                    break;
            }
        }

        // SOLID
        private string GetSolidWallImagePath(EWallDirection direction)
        {
            String wall = null;
            switch (direction)
            {
                case EWallDirection.North:
                    wall += ImageDirectory + "wall_north_solid.png";
                    break;
                case EWallDirection.East:
                    wall += ImageDirectory + "wall_east_solid.png";
                    break;
                case EWallDirection.South:
                    wall += ImageDirectory + "wall_north_solid.png";
                    break;
                case EWallDirection.West:
                    wall += ImageDirectory + "wall_west_solid.png";
                    break;
            }
            return wall;
        }
        // EMPTY
        private string GetEmptyWallImagePath(EWallDirection direction)
        {
            String wall = null;
            switch (direction)
            {
                case EWallDirection.North:
                    wall += ImageDirectory + "wall_north_empty.png";
                    break;
                case EWallDirection.East:
                    wall += ImageDirectory + "wall_east_empty.png";
                    break;
                case EWallDirection.South:
                    wall += ImageDirectory + "wall_north_empty.png";
                    break;
                case EWallDirection.West:
                    wall += ImageDirectory + "wall_west_empty.png";
                    break;
            }
            return wall;
        }

        // DOOR
        private string GetDoorImagePath(EWallDirection direction)
        {
            String wall = null;
            switch (direction)
            {
                case EWallDirection.North:
                    wall += ImageDirectory + "wall_north_door.png";
                    break;
                case EWallDirection.East:
                    wall += ImageDirectory + "wall_east_door.png";
                    break;
                case EWallDirection.South:
                    wall += ImageDirectory + "wall_north_door.png";
                    break;
                case EWallDirection.West:
                    wall += ImageDirectory + "wall_west_door.png";
                    break;
            }
            return wall;
        }
    }
}
