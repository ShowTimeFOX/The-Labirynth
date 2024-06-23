namespace GameLibrary
{
    public abstract class Item
    {
        public String Name { get; set; }
        public byte[] daneZdjecia;
        public EDirection polozenie1;
        public EDirection polozenie2;
        


        protected Item()
        {
        }

        protected Item(string name, byte[] daneZdjecia, EDirection polozenie1, EDirection polozenie2)
        {
            Name = name;
            this.daneZdjecia = daneZdjecia;
            this.polozenie1 = polozenie1;
            this.polozenie2 = polozenie2;
        }
    }
}
