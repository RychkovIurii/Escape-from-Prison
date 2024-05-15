using System;
namespace Prison
{
    public class Character
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public Dictionary<string, Action<Player>> Actions { get; set; }

        public Character(string name, string description)
        {
            Name = name;
            Description = description;
            Actions = new Dictionary<string, Action<Player>>();
        }
    }
}

