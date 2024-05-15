using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Prison
{
    public class Room
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public Dictionary<string, Room> Exits { get; private set; }
        public Dictionary<string, bool> DoorsOpen { get; private set; }
        public List<Item> Items { get; set; }
        public List<Character> Characters { get; set; }

        public Room(string name, string description)
        {
            Name = name;
            Description = description;
            Exits = new Dictionary<string, Room>();
            DoorsOpen = new Dictionary<string, bool>();
            Items = new List<Item>();
            Characters = new List<Character>();
        }

        public void SetExit(string direction, Room room, bool isOpen = false)
        {
            Exits[direction] = room;
            DoorsOpen[direction] = isOpen;
        }
        public void PrintDescription()
        {
            Console.WriteLine(Description);
            if (Items.Count > 0)
            {
                Console.WriteLine("\n\tYou see objects:");
                foreach (var item in Items)
                {
                    Console.WriteLine("\t- " + item.Name + ": " + item.Description);
                }
            }
            Console.WriteLine("\n\tThere are directions:");
            foreach (var exit in Exits)
            {
                Console.WriteLine("\t- " + exit.Key + ": " + exit.Value.Name);
            }
        }
        public bool ContainsItem(string itemName)
        {
            foreach (var item in Items)
            {
                if (item.Name.ToLower() == itemName.ToLower())
                {
                    return true;
                }
            }
            return false;
        }

        public Item TakeItem(string itemName)
        {
            for (int i = 0; i < Items.Count; i++)
            {
                if (Items[i].Name.ToLower() == itemName.ToLower())
                {
                    Item item = Items[i];
                    Items.RemoveAt(i);
                    return item;
                }
            }
            return null;
        }
        /*public void PerformRoomAction(Player player)
        {
            Console.WriteLine($"\tYou are in the {Description}.");

            switch (Name.ToLower())
            {
                case "cell":
                    if (!player.Inventory.Any(i => i.Name == "Key"))
                    {
                        Console.WriteLine("Under the mattress, you find a key and several cigarettes.");
                        player.AddToInventory(new Item("Key", "A key to the door leading to the canteen."));
                        player.AddToInventory(new Item("Cigarette", "A cigarette."));
                        player.AddToInventory(new Item("Cigarette", "A cigarette."));
                        player.AddToInventory(new Item("Cigarette", "A cigarette."));
                    }
                    break;

                case "corridor":
                    Console.WriteLine("Here you can buy alcohol or additional cigarettes from your fellow inmate.");
                    break;

                case "canteen":
                    Console.WriteLine("Here you can play blackjack with other inmates. Your winnings can be used for purchases in the corridor.");
                    break;

                case "yard":
                    Console.WriteLine("You can attempt to bribe the guard if you have alcohol.");
                    break;

                default:
                    Console.WriteLine("This place is not yet defined in the game.");
                    break;
            }
        }*/
    }
}

