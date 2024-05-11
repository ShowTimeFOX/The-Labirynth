using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameLibrary
{
    public class Coordinates
    {
        public int XCoordinete { get; set; }
        public int YCoordinate { get; set; }

        public Coordinates(int xCoordinete, int yCoordinate)
        {
            XCoordinete = xCoordinete;
            YCoordinate = yCoordinate;
        }

        public override string ToString()
        {
            return $"X: {XCoordinete}; Y: {YCoordinate}";
        }
    }
}
