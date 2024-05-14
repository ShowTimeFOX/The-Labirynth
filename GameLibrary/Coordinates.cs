using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameLibrary
{
    public class Coordinates
    {
        public int XCoordinate { get; set; }
        public int YCoordinate { get; set; }

        public Coordinates(int xCoordinete, int yCoordinate)
        {
            XCoordinate = xCoordinete;
            YCoordinate = yCoordinate;
        }

        public override string ToString()
        {
            return $"X: {XCoordinate}; Y: {YCoordinate}";
        }
    }
}
