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

        public Coordinates(int xCoordinate, int yCoordinate)
        {
            XCoordinate = xCoordinate;
            YCoordinate = yCoordinate;
        }

        public override string ToString()
        {
            return $"X: {XCoordinate}; Y: {YCoordinate}";
        }

        public override bool Equals(object? obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            Coordinates coordinates = (Coordinates)obj;
            return XCoordinate == coordinates.XCoordinate && YCoordinate == coordinates.YCoordinate;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(XCoordinate, YCoordinate);
        }
    }
}
