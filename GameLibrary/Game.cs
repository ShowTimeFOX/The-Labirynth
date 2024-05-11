namespace GameLibrary
{
    public class Game
    {
        Room[,] labirynth = new Room[6, 6]; //tutaj na sztywno
        private Player player; // Gracz
        public Game(Player player)
        {
            for (int i = 0; i < labirynth.GetLength(0); i++)
            {
                for (int j = 0; j < labirynth.GetLength(1); j++)
                {
                    // Tworzenie ścian dla każdego pokoju
                    Wall northWall = new Wall(EWallDirection.North, EWallType.Door);
                    Wall eastWall = new Wall(EWallDirection.East, EWallType.Door);
                    Wall southWall = new Wall(EWallDirection.South, EWallType.Door);
                    Wall westWall = new Wall(EWallDirection.West, EWallType.Door);

                    // Inicjalizacja pokoju i dodanie do tablicy
                    labirynth[i, j] = new Room(new Coordinates(i, j), northWall, eastWall, southWall, westWall);
                }
            }


            player.Coordinates = new Coordinates(0, 0);
        }

        public Coordinates GetCurrentCoordinates()
        {
            return player.Coordinates;
        }


    }
}
