using System;
using System.Drawing;
using System.IO;
using static System.Net.Mime.MediaTypeNames;

namespace GameLibrary
{
    public static class Compass
    {
        // Ścieżki do plików PNG z obrazami kompasu dla różnych kierunków
        private static readonly string g = Path.Combine("..", "..", "..", "..", "img/compass_pointer.png");
        private static readonly string PathNorth = g+"compass_north.png";
        private static readonly string PathSouth = g+"compass_south.png";
        private static readonly string PathEast = g+"compass_east.png";
        private static readonly string PathWest = g + "compass_west.png";
        public static string Current { get; set; }

        /// <summary>
        /// Ustawia obrót igły kompasu na określony kierunek.
        /// </summary>
        /// <param name="direction">Kierunek: "N" (północ), "S" (południe), "E" (wschód), "W" (zachód)</param>
        public static void SetNeedleDirection(EDirection direction)
        {
            string imagePath;

            switch (direction)
            {
                case EDirection.North:
                    imagePath = PathNorth;
                    break;
                case EDirection.South:
                    imagePath = PathSouth;
                    break;
                case EDirection.East:
                    imagePath = PathEast;
                    break;
                case EDirection.West:
                    imagePath = PathWest;
                    break;
                default:
                    Console.WriteLine("Nieprawidłowy kierunek.");
                    return;
            }

            Current = imagePath;


        }
    }
}