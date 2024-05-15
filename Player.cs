using System;
using System.Collections.Generic;
using System.Linq;
namespace Prison
{
    public class Player
    {
        public Room CurrentRoom { get; set; }
        public List<Item> Inventory { get; set; }
        private Game game;

        public Player(Game game, Room startRoom)
        {
            this.game = game;
            this.CurrentRoom = startRoom;
            Inventory = new List<Item>();
        }

        public void MoveTo(Room room)
        {
            CurrentRoom = room;
            Console.WriteLine($"\tYou moved to {room.Name}.");
            game.CheckForVictory(this);
        }

        public void AddToInventory(Item item)
        {
            var existingItem = Inventory.FirstOrDefault(i => i.Name == item.Name);
            if (existingItem != null)
            {
                existingItem.Quantity += item.Quantity;
            }
            else
            {
                Inventory.Add(item);
            }
        }

        public void RemoveFromInventory(string itemName, int quantity = 1)
        {
            var item = Inventory.FirstOrDefault(i => i.Name == itemName);
            if (item != null && item.Quantity >= quantity)
            {
                item.Quantity -= quantity;
                if (item.Quantity == 0)
                {
                    Inventory.Remove(item);
                }
            }
            else
            {
                Console.WriteLine("\tYou don't have it.");
            }
        }

        public void TakeItem(string itemName)
        {
            if (CurrentRoom.ContainsItem(itemName))
            {
                Item item = CurrentRoom.TakeItem(itemName);
                if (item != null)
                {
                    AddToInventory(item);
                    Console.WriteLine("\tYou took " + item.Name);
                }
                else
                {
                    Console.WriteLine("\tDidn't take it.");
                }
            }
            else
            {
                Console.WriteLine("\tThere is no this object.");
            }
        }

        public void UseItem(string itemName)
        {
            Item item = Inventory.FirstOrDefault(i => i.Name.Equals(itemName, StringComparison.OrdinalIgnoreCase));
            if (item == null)
            {
                Console.WriteLine("\tYou don't have " + itemName + ".");
                return;
            }

            if (item.Name.ToLower() == "key" && CurrentRoom.Name == "Cell" && CurrentRoom.Exits.ContainsKey("corridor"))
            {
                CurrentRoom.DoorsOpen["corridor"] = true;
                Console.WriteLine("\tYou used the Key to open the door to the corridor.");
            }
            else if (item.Name.ToLower() == "access card" && CurrentRoom.Name == "Yard" && CurrentRoom.Exits.ContainsKey("freedom"))
            {
                CurrentRoom.DoorsOpen["freedom"] = true;
                Console.WriteLine("\tYou used the Access Card to open the gate to freedom.");
            }
            else if (item.Name.ToLower() == "vodka")
            {
                if (CurrentRoom.Name == "Yard" && CurrentRoom.Characters.Any(c => c.Name.ToLower() == "guard"))
                {
                    Console.WriteLine("\tYou offered vodka to the guard. He's drunk and sloppy now.");
                    AddToInventory(new Item("Access Card", "A card that opens the gate to freedom.", 1));
                    Console.WriteLine("\tYou got the ACCESS CARD.");
                }
                else
                {
                    Console.WriteLine("\tYou drank the vodka yourself by mistake. You feel dizzy now.");
                }
                RemoveFromInventory(item.Name, 1);
            }
            else
            {
                Console.WriteLine("\tYou can't use " + itemName + " here.");
            }
        }

        public void ShowInventory()
        {
            Console.WriteLine("\nYour Inventory:");
            if (Inventory.Count == 0)
            {
                Console.WriteLine("Inventory is empty.");
            }
            else
            {
                foreach (var item in Inventory)
                {
                    Console.WriteLine($"{item.Name} - {item.Description} (Quantity: {item.Quantity})");
                }
            }
        }
    }
}

