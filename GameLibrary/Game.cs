namespace GameLibrary
{
    public class Game
    {
        private Room[,] labirynth = new Room[3,3];

        public Room[,] Labirynth
        {
            get { return labirynth; }
            set { labirynth = value; }
        }

         //tutaj na sztywno
        private Player player; // Gracz
        public Game(Player player)
        {
            //for (int i = 0; i < labirynth.GetLength(0); i++)
            //{
            //    for (int j = 0; j < labirynth.GetLength(1); j++)
            //    {
            //        // Tworzenie ścian dla każdego pokoju

            //        Wall[] walls = new Wall[]
            //        {
            //            new Wall(EWallDirection.North, EWallType.Door),
            //            new Wall(EWallDirection.East, EWallType.Door),
            //            new Wall(EWallDirection.South, EWallType.Door),
            //            new Wall(EWallDirection.West, EWallType.Door)
            //        };
            //        // Inicjalizacja pokoju i dodanie do tablicy
            //        labirynth[i, j] = new Room(new Coordinates(i, j), walls);
            //    }

            //}




            // Inicjalizacja labiryntu 2x2
            //Room[,] labirynth = new Room[2, 2];

            // Inicjalizacja pierwszego pokoju
            Wall[] wallsRoom1 = new Wall[]
            {
                new Wall(EWallDirection.North, EWallType.Door),
                new Wall(EWallDirection.East, EWallType.Solid),
                new Wall(EWallDirection.South, EWallType.Door),
                new Wall(EWallDirection.West, EWallType.Solid)
            };
            labirynth[0, 0] = new Room(new Coordinates(0, 0), wallsRoom1);

            Wall[] wallsRoom2 = new Wall[]
            {
                new Wall(EWallDirection.North, EWallType.Empty),
                new Wall(EWallDirection.East, EWallType.Solid),
                new Wall(EWallDirection.South, EWallType.Door),
                new Wall(EWallDirection.West, EWallType.Solid)
            };
            labirynth[0, 1] = new Room(new Coordinates(0, 0), wallsRoom2);

            Wall[] wallsRoom3 = new Wall[]
            {
                new Wall(EWallDirection.North, EWallType.Solid),
                new Wall(EWallDirection.East, EWallType.Solid),
                new Wall(EWallDirection.South, EWallType.Door),
                new Wall(EWallDirection.West, EWallType.Solid)
            };
            labirynth[0, 2] = new Room(new Coordinates(0, 0), wallsRoom3);

            player.Coordinates = new Coordinates(0, 0);
            player.Direction = EDirection.North;
        }
    }
}
