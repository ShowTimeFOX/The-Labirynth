using Microsoft.VisualBasic;
using System.Drawing;

namespace GameLibrary
{
    public class Game
    {
        private Room[,] labirynth = new Room[3,3];
        // ten słownik przechowuje stringa z nazwą ściany i danymi danej (hehe) ściany w pamięci
        //Jest to po prostu implementacja wyświetlania zdjęć nie z dysku tylko RAM'u
        //zrobiłem to bo myślałem, że to było powodem, że się ściany nie wyświetlały,
        //ale to nie był problem
        //więc może zostać z poprzedniej wersji z dysku albo zostawić to tak jak
        //teraz jest i wczytywać to do RAMU'u
        private Dictionary<string, byte[]> imageCache = new Dictionary<string, byte[]>();
        string ImageDirectory = Path.Combine("..", "..", "..", "..", "img/"); //TODO sciezka wzgledna do projektu

        

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

            
            //Wczytywanie zdjęć do pamięci RAM
            imageCache["wall_east_door.png"] = File.ReadAllBytes(Path.Combine(ImageDirectory, "wall_east_door.png"));
            imageCache["wall_east_empty.png"] = File.ReadAllBytes(Path.Combine(ImageDirectory, "wall_east_empty.png"));
            imageCache["wall_east_solid.png"] = File.ReadAllBytes(Path.Combine(ImageDirectory, "wall_east_solid.png"));
            imageCache["wall_north_door.png"] = File.ReadAllBytes(Path.Combine(ImageDirectory, "wall_north_door.png"));
            imageCache["wall_north_empty.png"] = File.ReadAllBytes(Path.Combine(ImageDirectory, "wall_north_empty.png"));
            imageCache["wall_north_solid.png"] = File.ReadAllBytes(Path.Combine(ImageDirectory, "wall_north_solid.png"));
            imageCache["wall_west_door.png"] = File.ReadAllBytes(Path.Combine(ImageDirectory, "wall_west_door.png"));
            imageCache["wall_west_empty.png"] = File.ReadAllBytes(Path.Combine(ImageDirectory, "wall_west_empty.png"));
            imageCache["wall_west_solid.png"] = File.ReadAllBytes(Path.Combine(ImageDirectory, "wall_west_solid.png"));
            imageCache["floor.png"] = File.ReadAllBytes(Path.Combine(ImageDirectory, "floor.png"));

            //Metoda co tworzy labirynt z pokoi, przeniosłem do oddzielnej metody
            //żeby śmietnika nie robić w konstruktorze
            this.CreateLabyrynth();
            player.Coordinates = new Coordinates(0, 0);
            player.Direction = EDirection.North;
        }

        private void CreateLabyrynth()
        {
            //inicjalizacja i dodanie do listy 3 pokoi zgodnych z tym naszym rysunkiem z discorda
            Wall[] wallsRoomX0Y0 = new Wall[]
            {
                new Wall(EWallDirection.North,EWallType.Empty),
                new Wall(EWallDirection.East,  EWallType.Door),
                new Wall(EWallDirection.South, EWallType.Door),
                new Wall(EWallDirection.West, EWallType.Solid)
            };
            labirynth[0, 0] = new Room(new Coordinates(0, 0), wallsRoomX0Y0);

            Wall[] wallsRoomX0Y1 = new Wall[]
            {
                new Wall(EWallDirection.North, EWallType.Empty),
                new Wall(EWallDirection.East, EWallType.Empty),
                new Wall(EWallDirection.South, EWallType.Empty),
                new Wall(EWallDirection.West, EWallType.Solid)
            };
            labirynth[0, 1] = new Room(new Coordinates(0, 0), wallsRoomX0Y1);

            Wall[] wallsRoomX0Y2 = new Wall[]
            {
                new Wall(EWallDirection.North, EWallType.Solid),
                new Wall(EWallDirection.East, EWallType.Empty),
                new Wall(EWallDirection.South, EWallType.Empty),
                new Wall(EWallDirection.West, EWallType.Solid)
            };
            labirynth[0, 2] = new Room(new Coordinates(0, 0), wallsRoomX0Y2);
        }
        public byte[] GetWall(EDirection viewDirection, EDirection playerDirection,int x, int y)
        {
            byte[] ImageToReturn = null;
            EWallType wallType = labirynth[x,y].Walls[(int)playerDirection].WallType;

            

            switch (wallType)
            {
                case EWallType.Solid:
                    ImageToReturn = GetSolidWallImage(viewDirection);
                    return ImageToReturn;
                case EWallType.Empty:
                    ImageToReturn = GetEmptyWallImage(viewDirection);
                    return ImageToReturn;
                case EWallType.Door:
                    ImageToReturn = GetDoorImage(viewDirection);
                    return ImageToReturn;
                default: return ImageToReturn;
            }
            
        }
        //To mówi samo za siebie
        public byte[] GetFloor()
        {
            return imageCache["floor.png"];
        }
        // SOLID
        private byte[] GetSolidWallImage(EDirection direction)
        {
            String wall = null;
            switch (direction)
            {
                case EDirection.North:
                    wall = "wall_north_solid.png";
                    break;
                case EDirection.East:
                    wall = "wall_east_solid.png";
                    break;
                case EDirection.South:
                    wall = "wall_north_solid.png";
                    break;
                case EDirection.West:
                    wall = "wall_west_solid.png";
                    break;
            }
            byte[] imageData = imageCache[wall];
            System.Diagnostics.Debug.WriteLine($"{wall}");
            return imageData;
        }
        //EMPTY
        private byte[] GetEmptyWallImage(EDirection direction)
        {
            string imageName = null;
            switch (direction)
            {
                case EDirection.North:
                    imageName = "wall_north_empty.png";
                    break;
                case EDirection.East:
                    imageName = "wall_east_empty.png";
                    break;
                case EDirection.South:
                    imageName = "wall_north_empty.png";
                    break;
                case EDirection.West:
                    imageName = "wall_west_empty.png";
                    break;
            }
            System.Diagnostics.Debug.WriteLine($"{imageName}");
            return imageCache[imageName];
        }

        // DOOR
        private byte[] GetDoorImage(EDirection direction)
        {
            string imageName = null;
            switch (direction)
            {
                case EDirection.North:
                    imageName = "wall_north_door.png";
                    break;
                case EDirection.East:
                    imageName = "wall_east_door.png";
                    break;
                case EDirection.South:
                    imageName = "wall_north_door.png";
                    break;
                case EDirection.West:
                    imageName = "wall_west_door.png";
                    break;
            }
            System.Diagnostics.Debug.WriteLine($"{imageName}");
            return imageCache[imageName];
        }
    }
}
