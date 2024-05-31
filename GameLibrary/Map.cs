using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace GameLibrary
{
    public class Map
    {
        public Dictionary<Coordinates, byte[]> mapFragments = new Dictionary<Coordinates, byte[]>();
        private string ImageDirectory = Path.Combine("..", "..", "..", "..", "map/");
        private byte[] bigMap;
        public List<Coordinates> discoveredMapCoordinates = new List<Coordinates>();
        public byte[] pointer;


        public byte[] BigMap
        {
            get { return bigMap; }
            set { bigMap = value; }
        }

        public Boolean isMapShown = false;

        public Map() 
        {

            bigMap = File.ReadAllBytes(Path.Combine(ImageDirectory, "duza mapa.png")); //Wczytanie do pamięci zdjęcia dużej mapy
            pointer = File.ReadAllBytes(Path.Combine(ImageDirectory, "pointer.png")); //Wczytanie do pamięci wskaźnika pozycji gracza

            // Wczytanie do pamięci fragmentów mapy
            mapFragments[new Coordinates(0, 0)] = File.ReadAllBytes(Path.Combine(ImageDirectory, "0x0.png"));
            mapFragments[new Coordinates(0, 1)] = File.ReadAllBytes(Path.Combine(ImageDirectory, "0x1.png"));
            mapFragments[new Coordinates(0, 2)] = File.ReadAllBytes(Path.Combine(ImageDirectory, "0x2.png"));
            mapFragments[new Coordinates(0, 3)] = File.ReadAllBytes(Path.Combine(ImageDirectory, "0x3.png"));
            mapFragments[new Coordinates(0, 4)] = File.ReadAllBytes(Path.Combine(ImageDirectory, "0x4.png"));
            mapFragments[new Coordinates(0, 5)] = File.ReadAllBytes(Path.Combine(ImageDirectory, "0x5.png"));

            mapFragments[new Coordinates(1, 0)] = File.ReadAllBytes(Path.Combine(ImageDirectory, "1x0.png"));
            mapFragments[new Coordinates(1, 1)] = File.ReadAllBytes(Path.Combine(ImageDirectory, "1x1.png"));
            mapFragments[new Coordinates(1, 2)] = File.ReadAllBytes(Path.Combine(ImageDirectory, "1x2.png"));
            mapFragments[new Coordinates(1, 3)] = File.ReadAllBytes(Path.Combine(ImageDirectory, "1x3.png"));
            mapFragments[new Coordinates(1, 4)] = File.ReadAllBytes(Path.Combine(ImageDirectory, "1x4.png"));
            mapFragments[new Coordinates(1, 5)] = File.ReadAllBytes(Path.Combine(ImageDirectory, "1x5.png"));

            mapFragments[new Coordinates(2, 0)] = File.ReadAllBytes(Path.Combine(ImageDirectory, "2x0.png"));
            mapFragments[new Coordinates(2, 1)] = File.ReadAllBytes(Path.Combine(ImageDirectory, "2x1.png"));
            mapFragments[new Coordinates(2, 2)] = File.ReadAllBytes(Path.Combine(ImageDirectory, "2x2.png"));
            mapFragments[new Coordinates(2, 3)] = File.ReadAllBytes(Path.Combine(ImageDirectory, "2x3.png"));
            mapFragments[new Coordinates(2, 4)] = File.ReadAllBytes(Path.Combine(ImageDirectory, "2x4.png"));
            mapFragments[new Coordinates(2, 5)] = File.ReadAllBytes(Path.Combine(ImageDirectory, "2x5.png"));

            mapFragments[new Coordinates(3, 0)] = File.ReadAllBytes(Path.Combine(ImageDirectory, "3x0.png"));
            mapFragments[new Coordinates(3, 1)] = File.ReadAllBytes(Path.Combine(ImageDirectory, "3x1.png"));
            mapFragments[new Coordinates(3, 2)] = File.ReadAllBytes(Path.Combine(ImageDirectory, "3x2.png"));
            mapFragments[new Coordinates(3, 3)] = File.ReadAllBytes(Path.Combine(ImageDirectory, "3x3.png"));
            mapFragments[new Coordinates(3, 4)] = File.ReadAllBytes(Path.Combine(ImageDirectory, "3x4.png"));
            mapFragments[new Coordinates(3, 5)] = File.ReadAllBytes(Path.Combine(ImageDirectory, "3x5.png"));

            mapFragments[new Coordinates(4, 0)] = File.ReadAllBytes(Path.Combine(ImageDirectory, "4x0.png"));
            mapFragments[new Coordinates(4, 1)] = File.ReadAllBytes(Path.Combine(ImageDirectory, "4x1.png"));
            mapFragments[new Coordinates(4, 2)] = File.ReadAllBytes(Path.Combine(ImageDirectory, "4x2.png"));
            mapFragments[new Coordinates(4, 3)] = File.ReadAllBytes(Path.Combine(ImageDirectory, "4x3.png"));
            mapFragments[new Coordinates(4, 4)] = File.ReadAllBytes(Path.Combine(ImageDirectory, "4x4.png"));
            mapFragments[new Coordinates(4, 5)] = File.ReadAllBytes(Path.Combine(ImageDirectory, "4x5.png"));

            mapFragments[new Coordinates(5, 0)] = File.ReadAllBytes(Path.Combine(ImageDirectory, "5x0.png"));
            mapFragments[new Coordinates(5, 1)] = File.ReadAllBytes(Path.Combine(ImageDirectory, "5x1.png"));
            mapFragments[new Coordinates(5, 2)] = File.ReadAllBytes(Path.Combine(ImageDirectory, "5x2.png"));
            mapFragments[new Coordinates(5, 3)] = File.ReadAllBytes(Path.Combine(ImageDirectory, "5x3.png"));
            mapFragments[new Coordinates(5, 4)] = File.ReadAllBytes(Path.Combine(ImageDirectory, "5x4.png"));
            mapFragments[new Coordinates(5, 5)] = File.ReadAllBytes(Path.Combine(ImageDirectory, "5x5.png"));

            discoveredMapCoordinates.Add(new Coordinates(0, 0));
        }

        //To nieudolnie zwraca odkryte fragmenty mapy
        //Tylko z porównaniem koordynatów tu jest problem bo mają różne instancje i dlatego to tak wygląda...
        public Dictionary<Coordinates, byte[]> GetDiscoveredMapFragments() //Nie patrz na to BO MI TAK WSTYD
        {
            Dictionary<Coordinates, byte[]> discoveredFragments = new Dictionary<Coordinates, byte[]>();

            foreach (Coordinates coord in discoveredMapCoordinates)
            {
                int X = coord.XCoordinate;
                int Y = coord.YCoordinate;

                // Przeszukaj wszystkie klucze w mapFragments
                foreach (var key in mapFragments.Keys)
                {
                    // Sprawdź, czy współrzędne klucza są równe współrzędnym koordynatów z odkrytych koordynatów XD
                    //To ZA KAŻDYM RAZEM łazi po całym słowniku...
                    //To nie ma sensu ale nie chce mi sie już dziś tego poprawiać...
                    if (key.XCoordinate == X && key.YCoordinate == Y)
                    {
                        discoveredFragments.Add(key, mapFragments[key]);
                        break; // Przerwij pętlę po dodaniu pasującego klucza
                    }
                }
            }

            return discoveredFragments;
        }

    }
}
