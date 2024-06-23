using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameLibrary
{
    public class ItemHealth : Item
    {
        public int HealthCapacity { get; set; }
        //tutaj moze byc cos i z sanity

        public ItemHealth(string name, byte[] daneZdjecia, int HealthCapacity, EDirection polozenie1, EDirection polozenie2)
            : base(name, daneZdjecia, polozenie1, polozenie2)
        {
            this.HealthCapacity = HealthCapacity;
        }
    }
}
