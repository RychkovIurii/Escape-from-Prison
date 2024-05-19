using System;
namespace Prison
{
    public class Item
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int Quantity { get; set; }

        public Item(string name, string description, int quantity = 1)
        {
            Name = name;
            Description = description;
            Quantity = quantity;
        }
    }
}
