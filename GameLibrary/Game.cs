using Microsoft.VisualBasic;
using System.Drawing;

namespace GameLibrary
{
    public class Game
    {
        private Room[,] labirynth = new Room[6,6];
        // ten słownik przechowuje stringa z nazwą ściany i danymi danej (hehe) ściany w pamięci
        //Jest to po prostu implementacja wyświetlania zdjęć nie z dysku tylko RAM'u
        //zrobiłem to bo myślałem, że to było powodem, że się ściany nie wyświetlały,
        //ale to nie był problem
        //więc może zostać z poprzedniej wersji z dysku albo zostawić to tak jak
        //teraz jest i wczytywać to do RAMU'u
        private Dictionary<string, byte[]> imageCache = new Dictionary<string, byte[]>();
        string ImageDirectory = Path.Combine("..", "..", "..", "..", "img/"); //TODO sciezka wzgledna do projektu
        public Map Map = new Map(); // Instancja mapy

        public Room[,] Labirynth
        {
            get { return labirynth; }
            set { labirynth = value; }
        }

         //tutaj na sztywno
        private Player player; // Gracz
        public Game(Player player)
        { 
            //Wczytywanie zdjęć do pamięci RAM
            // pokój
            imageCache["wall_east_door"] = File.ReadAllBytes(Path.Combine(ImageDirectory, "wall_east_door.png"));
            imageCache["wall_east_empty"] = File.ReadAllBytes(Path.Combine(ImageDirectory, "wall_east_empty.png"));
            imageCache["wall_east_solid"] = File.ReadAllBytes(Path.Combine(ImageDirectory, "wall_east_solid.png"));
            imageCache["wall_north_door"] = File.ReadAllBytes(Path.Combine(ImageDirectory, "wall_north_door.png"));
            imageCache["wall_north_empty"] = File.ReadAllBytes(Path.Combine(ImageDirectory, "wall_north_empty.png"));
            imageCache["wall_north_solid"] = File.ReadAllBytes(Path.Combine(ImageDirectory, "wall_north_solid.png"));
            imageCache["wall_west_door"] = File.ReadAllBytes(Path.Combine(ImageDirectory, "wall_west_door.png"));
            imageCache["wall_west_empty"] = File.ReadAllBytes(Path.Combine(ImageDirectory, "wall_west_empty.png"));
            imageCache["wall_west_solid"] = File.ReadAllBytes(Path.Combine(ImageDirectory, "wall_west_solid.png"));
            imageCache["floor.png"] = File.ReadAllBytes(Path.Combine(ImageDirectory, "floor.png"));

            imageCache["red_key.png"] = File.ReadAllBytes(Path.Combine(ImageDirectory, "red_key.png"));
            imageCache["green_key.png"] = File.ReadAllBytes(Path.Combine(ImageDirectory, "green_key.png"));
            imageCache["blue_key.png"] = File.ReadAllBytes(Path.Combine(ImageDirectory, "blue_key.png"));
            imageCache["yellow_key.png"] = File.ReadAllBytes(Path.Combine(ImageDirectory, "yellow_key.png"));
            imageCache["hp.png"] = File.ReadAllBytes(Path.Combine(ImageDirectory, "hp.png"));
            imageCache["knife.png"] = File.ReadAllBytes(Path.Combine(ImageDirectory, "knife.png"));

            imageCache["wall_east_door_blue"] = File.ReadAllBytes(Path.Combine(ImageDirectory, "wall_east_door_blue.png"));
            imageCache["wall_east_door_gold"] = File.ReadAllBytes(Path.Combine(ImageDirectory, "wall_east_door_gold.png"));
            imageCache["wall_east_door_green"] = File.ReadAllBytes(Path.Combine(ImageDirectory, "wall_east_door_green.png"));
            imageCache["wall_east_door_red"] = File.ReadAllBytes(Path.Combine(ImageDirectory, "wall_east_door_red.png"));

            imageCache["wall_north_door_blue"] = File.ReadAllBytes(Path.Combine(ImageDirectory, "wall_north_door_blue.png"));
            imageCache["wall_north_door_gold"] = File.ReadAllBytes(Path.Combine(ImageDirectory, "wall_north_door_gold.png"));
            imageCache["wall_north_door_green"] = File.ReadAllBytes(Path.Combine(ImageDirectory, "wall_north_door_green.png"));
            imageCache["wall_north_door_red"] = File.ReadAllBytes(Path.Combine(ImageDirectory, "wall_north_door_red.png"));

            imageCache["wall_west_door_blue"] = File.ReadAllBytes(Path.Combine(ImageDirectory, "wall_west_door_blue.png"));
            imageCache["wall_west_door_gold"] = File.ReadAllBytes(Path.Combine(ImageDirectory, "wall_west_door_gold.png"));
            imageCache["wall_west_door_green"] = File.ReadAllBytes(Path.Combine(ImageDirectory, "wall_west_door_green.png"));
            imageCache["wall_west_door_red"] = File.ReadAllBytes(Path.Combine(ImageDirectory, "wall_west_door_red.png"));
            //potwory

            //itemy


            this.CreateLabyrynth();
            ////////////////ZMIENIC//////////////////////////////////////////////////////////////////////////////////////////
            player.Coordinates = new Coordinates(0, 0);
            //player.Coordinates = new Coordinates(4,5);
            player.Direction = EDirection.North;
        }

        private void CreateLabyrynth()
        {
            //inicjalizacja i dodanie do listy pokoi zgodnych z tym naszym rysunkiem z discorda
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
            //Monster m01 = new Monster("Pepe", Path.Combine("..", "..", "..", "..", "img/pepe.png"), 20, 100, 10, 60);
            //labirynth[0, 1] = new Room(new Coordinates(0, 0), wallsRoomX0Y1, true, m01);
            labirynth[0, 1] = new Room(new Coordinates(0, 0), wallsRoomX0Y1);

            Wall[] wallsRoomX0Y2 = new Wall[]
            {
                new Wall(EWallDirection.North, EWallType.Solid),
                new Wall(EWallDirection.East, EWallType.Empty),
                new Wall(EWallDirection.South, EWallType.Empty),
                new Wall(EWallDirection.West, EWallType.Solid)
            };

            Monster m02 = new Monster("Dogge", Path.Combine("..", "..", "..", "..", "img/dogge.png"), 20, 100, 20, 30);
            //labirynth[0, 2] = new Room(new Coordinates(0, 0), wallsRoomX0Y2, true, m02);
            labirynth[0, 2] = new Room(new Coordinates(0, 0), wallsRoomX0Y2);

            Wall[] wallsRoomX0Y3 = new Wall[]
            {
                new Wall(EWallDirection.North, EWallType.Solid),
                new Wall(EWallDirection.East, EWallType.Door),
                new Wall(EWallDirection.South, EWallType.Solid),
                new Wall(EWallDirection.West, EWallType.Solid)
            };

           
            labirynth[0, 3] = new Room(new Coordinates(0, 0), wallsRoomX0Y3);
            Item kluczyk_czerwony = new ItemLock("kluczyk_czerwony", imageCache["red_key.png"],EDirection.North, EDirection.West);
            labirynth[0, 3].item = kluczyk_czerwony;


            Wall[] wallsRoomX0Y4 = new Wall[]
            {
                new Wall(EWallDirection.North, EWallType.Empty),
                new Wall(EWallDirection.East, EWallType.Empty),
                new Wall(EWallDirection.South, EWallType.Solid),
                new Wall(EWallDirection.West, EWallType.Solid)
            };
            labirynth[0, 4] = new Room(new Coordinates(0, 0), wallsRoomX0Y4);

            Wall[] wallsRoomX0Y5 = new Wall[]
            {
                new Wall(EWallDirection.North, EWallType.Solid),
                new Wall(EWallDirection.East, EWallType.Empty),
                new Wall(EWallDirection.South, EWallType.Empty),
                new Wall(EWallDirection.West, EWallType.Solid)
            };
            labirynth[0, 5] = new Room(new Coordinates(0, 0), wallsRoomX0Y5);

            //ROW 1
            Wall[] wallsRoomX1Y0 = new Wall[]
            {
                new Wall(EWallDirection.North, EWallType.Solid),
                new Wall(EWallDirection.East, EWallType.Solid),
                new Wall(EWallDirection.South, EWallType.Solid),
                new Wall(EWallDirection.West, EWallType.Door)
            };
            labirynth[1, 0] = new Room(new Coordinates(0, 0), wallsRoomX1Y0);

            Wall[] wallsRoomX1Y1 = new Wall[]
            {
                new Wall(EWallDirection.North, EWallType.Solid),
                new Wall(EWallDirection.East, EWallType.Empty),
                new Wall(EWallDirection.South, EWallType.Solid),
                new Wall(EWallDirection.West, EWallType.Empty)
            };
            labirynth[1, 1] = new Room(new Coordinates(0, 0), wallsRoomX1Y1);

            Wall[] wallsRoomX1Y2 = new Wall[]
            {
                new Wall(EWallDirection.North, EWallType.RedDoor),
                new Wall(EWallDirection.East, EWallType.Solid),
                new Wall(EWallDirection.South, EWallType.Solid),
                new Wall(EWallDirection.West, EWallType.Empty)
            };
            
            labirynth[1, 2] = new Room(new Coordinates(0, 0), wallsRoomX1Y2);
            Item kluczyk_zielony = new ItemLock("kluczyk_zielony", imageCache["green_key.png"], EDirection.North, EDirection.West);
            labirynth[1, 2].item = kluczyk_zielony;

            Wall[] wallsRoomX1Y3 = new Wall[]
            {
                new Wall(EWallDirection.North, EWallType.Solid),
                new Wall(EWallDirection.East, EWallType.Empty),
                new Wall(EWallDirection.South, EWallType.RedDoor),
                new Wall(EWallDirection.West, EWallType.Door)
            };
            labirynth[1, 3] = new Room(new Coordinates(0, 0), wallsRoomX1Y3);

            Wall[] wallsRoomX1Y4 = new Wall[]
            {
                new Wall(EWallDirection.North, EWallType.Solid),
                new Wall(EWallDirection.East, EWallType.BlueDoor),
                new Wall(EWallDirection.South, EWallType.Solid),
                new Wall(EWallDirection.West, EWallType.Empty)
            };
            labirynth[1, 4] = new Room(new Coordinates(0, 0), wallsRoomX1Y4);

            Wall[] wallsRoomX1Y5 = new Wall[]
            {
                new Wall(EWallDirection.North, EWallType.Solid),
                new Wall(EWallDirection.East, EWallType.Empty),
                new Wall(EWallDirection.South, EWallType.Solid),
                new Wall(EWallDirection.West, EWallType.Empty)
            };
            labirynth[1, 5] = new Room(new Coordinates(0, 0), wallsRoomX1Y5);

            //ROW 2
            Wall[] wallsRoomX2Y0 = new Wall[]
            {
                new Wall(EWallDirection.North, EWallType.Door),
                new Wall(EWallDirection.East, EWallType.Empty),
                new Wall(EWallDirection.South, EWallType.Solid),
                new Wall(EWallDirection.West, EWallType.Solid)
            };
            labirynth[2, 0] = new Room(new Coordinates(0, 0), wallsRoomX2Y0);

            Wall[] wallsRoomX2Y1 = new Wall[]
            {
                new Wall(EWallDirection.North, EWallType.GreenDoor),
                new Wall(EWallDirection.East, EWallType.Solid),
                new Wall(EWallDirection.South, EWallType.Door),
                new Wall(EWallDirection.West, EWallType.Empty)
            };
            labirynth[2, 1] = new Room(new Coordinates(0, 0), wallsRoomX2Y1);

            Wall[] wallsRoomX2Y2 = new Wall[]
            {
                new Wall(EWallDirection.North, EWallType.Solid),
                new Wall(EWallDirection.East, EWallType.Empty),
                new Wall(EWallDirection.South, EWallType.GreenDoor),
                new Wall(EWallDirection.West, EWallType.Solid)
            };
            
            labirynth[2, 2] = new Room(new Coordinates(0, 0), wallsRoomX2Y2);

            Wall[] wallsRoomX2Y3 = new Wall[]
            {
                new Wall(EWallDirection.North, EWallType.Empty),
                new Wall(EWallDirection.East, EWallType.Empty),
                new Wall(EWallDirection.South, EWallType.Solid),
                new Wall(EWallDirection.West, EWallType.Empty)
            };
            labirynth[2, 3] = new Room(new Coordinates(0, 0), wallsRoomX2Y3);

            Wall[] wallsRoomX2Y4 = new Wall[]
            {
                new Wall(EWallDirection.North, EWallType.Solid),
                new Wall(EWallDirection.East, EWallType.Solid),
                new Wall(EWallDirection.South, EWallType.Empty),
                new Wall(EWallDirection.West, EWallType.BlueDoor)
            };
            labirynth[2, 4] = new Room(new Coordinates(0, 0), wallsRoomX2Y4);

            Wall[] wallsRoomX2Y5 = new Wall[]
            {
                new Wall(EWallDirection.North, EWallType.Solid),
                new Wall(EWallDirection.East, EWallType.Empty),
                new Wall(EWallDirection.South, EWallType.Solid),
                new Wall(EWallDirection.West, EWallType.Empty)
            };
            labirynth[2, 5] = new Room(new Coordinates(0, 0), wallsRoomX2Y5);

            //ROW 3
            Wall[] wallsRoomX3Y0 = new Wall[]
            {
                new Wall(EWallDirection.North, EWallType.Solid),
                new Wall(EWallDirection.East, EWallType.Solid),
                new Wall(EWallDirection.South, EWallType.Solid),
                new Wall(EWallDirection.West, EWallType.Empty)
            };
            labirynth[3, 0] = new Room(new Coordinates(0, 0), wallsRoomX3Y0);
            Item apteczka1 = new ItemHealth("apteczka", imageCache["hp.png"],20, EDirection.East, EDirection.North);
            labirynth[3, 0].item = apteczka1;

            Wall[] wallsRoomX3Y1 = new Wall[]
            {
                new Wall(EWallDirection.North, EWallType.Empty),
                new Wall(EWallDirection.East, EWallType.Empty),
                new Wall(EWallDirection.South, EWallType.Solid),
                new Wall(EWallDirection.West, EWallType.Solid)
            };
            labirynth[3, 1] = new Room(new Coordinates(0, 0), wallsRoomX3Y1);

            Wall[] wallsRoomX3Y2 = new Wall[]
            {
                new Wall(EWallDirection.North, EWallType.Solid),
                new Wall(EWallDirection.East, EWallType.Solid),
                new Wall(EWallDirection.South, EWallType.Empty),
                new Wall(EWallDirection.West, EWallType.Empty)
            };
            labirynth[3, 2] = new Room(new Coordinates(0, 0), wallsRoomX3Y2);
            Item noz1 = new ItemDamage("noz1", imageCache["knife.png"], EDirection.South, EDirection.East);
            labirynth[3, 2].item = noz1;

            Wall[] wallsRoomX3Y3 = new Wall[]
            {
                new Wall(EWallDirection.North, EWallType.Solid),
                new Wall(EWallDirection.East, EWallType.Empty),
                new Wall(EWallDirection.South, EWallType.Solid),
                new Wall(EWallDirection.West, EWallType.Empty)
            };
            labirynth[3, 3] = new Room(new Coordinates(0, 0), wallsRoomX3Y3);


            Wall[] wallsRoomX3Y4 = new Wall[]
            {
                new Wall(EWallDirection.North, EWallType.Door),
                new Wall(EWallDirection.East, EWallType.Empty),
                new Wall(EWallDirection.South, EWallType.Solid),
                new Wall(EWallDirection.West, EWallType.Solid)
            };
            labirynth[3, 4] = new Room(new Coordinates(0, 0), wallsRoomX3Y4);
            Item apteczka3 = new ItemHealth("apteczka", imageCache["hp.png"], 40, EDirection.East, EDirection.North);
            labirynth[3, 4].item = apteczka3;

            Wall[] wallsRoomX3Y5 = new Wall[]
            {
                new Wall(EWallDirection.North, EWallType.Solid),
                new Wall(EWallDirection.East, EWallType.Empty),
                new Wall(EWallDirection.South, EWallType.Door),
                new Wall(EWallDirection.West, EWallType.Empty)
            };
            labirynth[3, 5] = new Room(new Coordinates(0, 0), wallsRoomX3Y5);

            //ROW 4
            Wall[] wallsRoomX4Y0 = new Wall[]
            {
                new Wall(EWallDirection.North, EWallType.Door),
                new Wall(EWallDirection.East, EWallType.Empty),
                new Wall(EWallDirection.South, EWallType.Solid),
                new Wall(EWallDirection.West, EWallType.Solid)
            };
            labirynth[4, 0] = new Room(new Coordinates(0, 0), wallsRoomX4Y0);

            Wall[] wallsRoomX4Y1 = new Wall[]
            {
                new Wall(EWallDirection.North, EWallType.Solid),
                new Wall(EWallDirection.East, EWallType.Empty),
                new Wall(EWallDirection.South, EWallType.Door),
                new Wall(EWallDirection.West, EWallType.Empty)
            };
            labirynth[4, 1] = new Room(new Coordinates(0, 0), wallsRoomX4Y1);

            Wall[] wallsRoomX4Y2 = new Wall[]
            {
                new Wall(EWallDirection.North, EWallType.Solid),
                new Wall(EWallDirection.East, EWallType.Solid),
                new Wall(EWallDirection.South, EWallType.Solid),
                new Wall(EWallDirection.West, EWallType.Solid)
            };
            labirynth[4, 2] = new Room(new Coordinates(0, 0), wallsRoomX4Y2);

            Wall[] wallsRoomX4Y3 = new Wall[]
            {
                new Wall(EWallDirection.North, EWallType.Solid),
                new Wall(EWallDirection.East, EWallType.Empty),
                new Wall(EWallDirection.South, EWallType.Solid),
                new Wall(EWallDirection.West, EWallType.Empty)
            };
            labirynth[4, 3] = new Room(new Coordinates(0, 0), wallsRoomX4Y3);

            Wall[] wallsRoomX4Y4 = new Wall[]
            {
                new Wall(EWallDirection.North, EWallType.Solid),
                new Wall(EWallDirection.East, EWallType.Solid),
                new Wall(EWallDirection.South, EWallType.Solid),
                new Wall(EWallDirection.West, EWallType.Empty)
            };
            labirynth[4, 4] = new Room(new Coordinates(0, 0), wallsRoomX4Y4);
            Item noz2 = new ItemDamage("noz2", imageCache["knife.png"], EDirection.South, EDirection.East);
            labirynth[4, 4].item = noz2;

            Wall[] wallsRoomX4Y5 = new Wall[]
            {
                new Wall(EWallDirection.North, EWallType.Solid),
                new Wall(EWallDirection.East, EWallType.Door), // TO JEST POTRZEBNE FABULARNIE (zamist solid)
                new Wall(EWallDirection.South, EWallType.Solid),
                new Wall(EWallDirection.West, EWallType.Empty)
            };
            labirynth[4, 5] = new Room(new Coordinates(0, 0), wallsRoomX4Y5);

            //ROW 5
            Wall[] wallsRoomX5Y0 = new Wall[]
            {
                new Wall(EWallDirection.North, EWallType.Solid),
                new Wall(EWallDirection.East, EWallType.Solid),
                new Wall(EWallDirection.South, EWallType.Solid),
                new Wall(EWallDirection.West, EWallType.Empty)
            };
            labirynth[5, 0] = new Room(new Coordinates(0, 0), wallsRoomX5Y0);
            Item kluczyk_zolty = new ItemLock("kluczyk_zolty", imageCache["yellow_key.png"], EDirection.North, EDirection.West);
            labirynth[5, 0].item = kluczyk_zolty;

            Wall[] wallsRoomX5Y1 = new Wall[]
            {
                new Wall(EWallDirection.North, EWallType.Empty),
                new Wall(EWallDirection.East, EWallType.Solid),
                new Wall(EWallDirection.South, EWallType.Solid),
                new Wall(EWallDirection.West, EWallType.Empty)
            };
            labirynth[5, 1] = new Room(new Coordinates(0, 0), wallsRoomX5Y1);

            Wall[] wallsRoomX5Y2 = new Wall[]
            {
                new Wall(EWallDirection.North, EWallType.YellowDoor),
                new Wall(EWallDirection.East, EWallType.Solid),
                new Wall(EWallDirection.South, EWallType.Empty),
                new Wall(EWallDirection.West, EWallType.Solid)
            };
            labirynth[5, 2] = new Room(new Coordinates(0, 0), wallsRoomX5Y2);
            Item apteczka2 = new ItemHealth("apteczka", imageCache["hp.png"], 40, EDirection.East, EDirection.North);
            labirynth[5, 2].item = apteczka2;

            Wall[] wallsRoomX5Y3 = new Wall[]
            {
                new Wall(EWallDirection.North, EWallType.Door),
                new Wall(EWallDirection.East, EWallType.Solid),
                new Wall(EWallDirection.South, EWallType.YellowDoor),
                new Wall(EWallDirection.West, EWallType.Empty)
            };
            Monster m03 = new Monster("Dogge", Path.Combine("..", "..", "..", "..", "img/dogge.png"), 80, 100, 30, 30);
            //labirynth[0, 3] = new Room(new Coordinates(0, 0), wallsRoomX0Y3, true, m03);
            labirynth[5, 3] = new Room(new Coordinates(0, 0), wallsRoomX5Y3,true,m03);


            Wall[] wallsRoomX5Y4 = new Wall[]
            {
                new Wall(EWallDirection.North, EWallType.Solid),
                new Wall(EWallDirection.East, EWallType.Solid),
                new Wall(EWallDirection.South, EWallType.Door),
                new Wall(EWallDirection.West, EWallType.Solid)
            };
            labirynth[5, 4] = new Room(new Coordinates(0, 0), wallsRoomX5Y4);
            Item kluczyk_niebieski = new ItemLock("kluczyk_niebieski", imageCache["blue_key.png"], EDirection.North, EDirection.West);
            labirynth[5, 4].item = kluczyk_niebieski;

            Wall[] wallsRoomX5Y5 = new Wall[]
            {
                new Wall(EWallDirection.North, EWallType.Solid),
                new Wall(EWallDirection.East, EWallType.Door),
                new Wall(EWallDirection.South, EWallType.Solid),
                new Wall(EWallDirection.West, EWallType.Empty)
            };
            Monster m55 = new Monster("POTĘŻNY BAZODANOWIEC", Path.Combine("..", "..", "..", "..", "img/finalboss.png"), 30, 200, 30, 30);
            labirynth[5, 5] = new Room(new Coordinates(0, 0), wallsRoomX5Y5, true, m55);
            //labirynth[5, 5] = new Room(new Coordinates(0, 0), wallsRoomX5Y5);
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
                case EWallType.GreenDoor:
                    ImageToReturn = GetGreenDoorImage(viewDirection);
                    return ImageToReturn;
                case EWallType.BlueDoor:
                    ImageToReturn = GetBlueDoorImage(viewDirection);
                    return ImageToReturn;
                case EWallType.RedDoor:
                    ImageToReturn = GetRedDoorImage(viewDirection);
                    return ImageToReturn;
                case EWallType.YellowDoor:
                    ImageToReturn = GetYellowDoorImage(viewDirection);
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
                    wall = "wall_north_solid";
                    break;
                case EDirection.East:
                    wall = "wall_east_solid";
                    break;
                case EDirection.South:
                    wall = "wall_north_solid";
                    break;
                case EDirection.West:
                    wall = "wall_west_solid";
                    break;
            }
            byte[] imageData = imageCache[wall];
            return imageData;
        }
        //EMPTY
        private byte[] GetEmptyWallImage(EDirection direction)
        {
            string imageName = null;
            switch (direction)
            {
                case EDirection.North:
                    imageName = "wall_north_empty";
                    break;
                case EDirection.East:
                    imageName = "wall_east_empty";
                    break;
                case EDirection.South:
                    imageName = "wall_north_empty";
                    break;
                case EDirection.West:
                    imageName = "wall_west_empty";
                    break;
            }
            return imageCache[imageName];
        }

        // DOOR
        private byte[] GetDoorImage(EDirection direction)
        {
            string imageName = null;
            switch (direction)
            {
                case EDirection.North:
                    imageName = "wall_north_door";
                    break;
                case EDirection.East:
                    imageName = "wall_east_door";
                    break;
                case EDirection.South:
                    imageName = "wall_north_door";
                    break;
                case EDirection.West:
                    imageName = "wall_west_door";
                    break;
            }
            return imageCache[imageName];
        }
        private byte[] GetGreenDoorImage(EDirection direction)
        {
            string imageName = null;
            switch (direction)
            {
                case EDirection.North:
                    imageName = "wall_north_door_green";
                    break;
                case EDirection.East:
                    imageName = "wall_east_door_green";
                    break;
                case EDirection.South:
                    imageName = "wall_north_door_green";
                    break;
                case EDirection.West:
                    imageName = "wall_west_door_green";
                    break;
            }
            return imageCache[imageName];
        }
        private byte[] GetBlueDoorImage(EDirection direction)
        {
            string imageName = null;
            switch (direction)
            {
                case EDirection.North:
                    imageName = "wall_north_door_blue";
                    break;
                case EDirection.East:
                    imageName = "wall_east_door_blue";
                    break;
                case EDirection.South:
                    imageName = "wall_north_door_blue";
                    break;
                case EDirection.West:
                    imageName = "wall_west_door_blue";
                    break;
            }
            return imageCache[imageName];
        }
        private byte[] GetRedDoorImage(EDirection direction)
        {
            string imageName = null;
            switch (direction)
            {
                case EDirection.North:
                    imageName = "wall_north_door_red";
                    break;
                case EDirection.East:
                    imageName = "wall_east_door_red";
                    break;
                case EDirection.South:
                    imageName = "wall_north_door_red";
                    break;
                case EDirection.West:
                    imageName = "wall_west_door_red";
                    break;
            }
            return imageCache[imageName];
        }
        private byte[] GetYellowDoorImage(EDirection direction)
        {
            string imageName = null;
            switch (direction)
            {
                case EDirection.North:
                    imageName = "wall_north_door_gold";
                    break;
                case EDirection.East:
                    imageName = "wall_east_door_gold";
                    break;
                case EDirection.South:
                    imageName = "wall_north_door_gold";
                    break;
                case EDirection.West:
                    imageName = "wall_west_door_gold";
                    break;
            }
            return imageCache[imageName];
        }


    }
}
