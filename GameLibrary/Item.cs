namespace GameLibrary
{
    public abstract class Item
    {
        public String Name { get; set; }
        public String ImagePath { get; set; }

        protected Item()
        {
        }

        protected Item(string name, string imagePath)
        {
            Name = name;
            ImagePath = imagePath;
        }
    }
}
