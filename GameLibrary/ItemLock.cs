using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameLibrary
{
    public class ItemLock : Item
    {
        public Coordinates CoordinatesToUnlock { get; set; }
        public EDirection DirectionToUnlock { get; set; }
        
    }
}
