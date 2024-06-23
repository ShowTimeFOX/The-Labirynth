using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameLibrary
{
    public class ItemLock : Item
    {
        public ItemLock(string name, byte[] daneZdjecia, EDirection polozenie1, EDirection polozenie2)
            : base(name, daneZdjecia,polozenie1,polozenie2)
        {
            
        }
    }
}
