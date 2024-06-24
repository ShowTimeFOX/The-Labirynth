namespace GameLibrary
{
    public abstract class Character
    {
        public String Name { get; set; }
        public String ImagePath { get; set; }
        public int HPCurrent { get; set; }
        public int HPMax { get; set; }
        public int Strength { get; set; }   //damage given
        public int Dexterity { get; set; }  //ability to avoid damage

        protected Character()
        {
        }

        protected Character(string name, string imagePath, int hPCurrent, int hPMax, int strength, int dexterity)
        {
            
            if(name.Length>= 30) throw new StringtooLongException("Twój stary za długi");
            Name = name;
            ImagePath = imagePath;
            HPCurrent = hPCurrent;
            
            HPMax = hPMax;
            Strength = strength;
            Dexterity = dexterity;
        }

        public override string ToString()
        {
            return $"Name: {Name}";
        }
    }
}
