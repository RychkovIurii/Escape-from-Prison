using System;
using System.Numerics;
using System.Diagnostics;

namespace Prison
{
	public class Game
    {
        private Player player;
        private Room startRoom;
        private Stopwatch stopwatch = new Stopwatch();

        public Game()
        {
            SetUpGame();
            player = new Player(this, startRoom);
        }

        private void SetUpGame()
        {
            Room cell = new Room("Cell", "\n\tA dark cell with a mattress.");
            Room corridor = new Room("Corridor", "\n\tA long corridor with other inmates. It seems like Bob sell something.");
            Room canteen = new Room("Canteen", "\n\tHere inmates gather to eat and play blackjack.");
            Room yard = new Room("Yard", "\n\tA spacious yard for walking, overseen by a guard.");
            Room freedom = new Room("Freedom", "\n\tYou've escaped the prison, breathe in your freedom!");

            cell.SetExit("corridor", corridor, false);
            corridor.SetExit("cell", cell, true);
            corridor.SetExit("canteen", canteen, true);
            corridor.SetExit("yard", yard, true);
            canteen.SetExit("corridor", corridor, true);
            yard.SetExit("corridor", corridor, true);
            yard.SetExit("freedom", freedom, false);

            startRoom = cell;

            cell.Items.Add(new Item("Key", "\tA key hidden under the mattress, opens the door to the corridor.", 1));
            cell.Items.Add(new Item("Cigarettes", "\tA few cigarettes hidden under the mattress.", 8));

            corridor.Characters.Add(new Character("Bob", "A shady looking inmate who can trade."));
            yard.Characters.Add(new Character("Guard", "A guard who loves alcohol."));
        }

        public void Play()
        {
            stopwatch.Start();
            Console.WriteLine("Welcome to the Escape from Prison game!");
            Console.WriteLine("Type 'help' for a list of commands.");
            Console.WriteLine("Cigarettes are very important! Don't loose all of them");
            player.CurrentRoom.PrintDescription();

            bool gameRunning = true;
            while (gameRunning)
            {
                /*Console.WriteLine("You are currently in the " + player.CurrentRoom.Name);*/
                Console.WriteLine();
                Console.Write("Your action: ");
                string command = Console.ReadLine().Trim().ToLower();

                if (string.IsNullOrEmpty(command))
                {
                    Console.WriteLine("\tPlease enter a command.");
                    continue;
                }

                if (command == "exit")
                {
                    Console.WriteLine("Are you sure you want to exit the game? (yes/no)");
                    string confirmation = Console.ReadLine().Trim().ToLower();
                    if (confirmation == "yes")
                    {
                        Console.WriteLine("Game over. Thank you for playing!");
                        gameRunning = false;
                    }
                    continue;
                }

                if (command == "help")
                {
                    PrintHelp();
                    continue;
                }

                bool result = ProcessCommand(command);
                if (!result)
                {
                    Console.WriteLine("\tInvalid command or action cannot be performed. Type 'help' for assistance.");
                }
            }
        }

        public void CheckForVictory(Player player)
        {
            string youWon = @"__   __             __          __         _ 
                              \ \ / /             \ \        / /        | |
                               \ V /___  _   _     \ \  /\  / /__  _ __ | |
                                \ // _ \| | | |     \ \/  \/ / _ \| '_ \| |
                                | | (_) | |_| |      \  /\  / (_) | | | |_|
                                \_/\___/ \__,_|       \/  \/ \___/|_| |_(_)
            ";
            if (player.CurrentRoom.Name.ToLower() == "freedom")
            {
                stopwatch.Stop();
                TimeSpan timeTaken = stopwatch.Elapsed;
                Console.WriteLine("\n\nCongratulations! You have successfully escaped the prison!");
                Console.WriteLine("Feel the fresh air of freedom and enjoy your life beyond these walls.");
                Console.WriteLine(youWon);
                Console.WriteLine($"Time taken to escape: {timeTaken.Hours}h {timeTaken.Minutes}m {timeTaken.Seconds}s");

                Environment.Exit(0);
            }
        }

        /*public void CheckInventory()
        {
            if (!player.Inventory.Any(item => item.Name == "Cigarettes" && item.Quantity == 0))
            {
                Console.WriteLine("\tYou have lost the cigarettes and cannot continue. I told you: Cigarettes are very important!");
                Environment.Exit(0);
            }
        }*/

        private bool ProcessCommand(string command)
        {
            string[] cmd = command.ToLower().Trim().Split(' ');
            string action = cmd[0];

            switch (action)
            {
                case "move":
                    return ProcessMoveCommand(cmd);
                case "take":
                    return ProcessTakeCommand(cmd);
                case "talk":
                    return ProcessTalkCommand(cmd);
                case "use":
                    return ProcessUseCommand(cmd);
                case "look":
                    return ProcessLookCommand();
                case "inventory":
                    player.ShowInventory();
                    return true;
                case "play":
                    return ProcessPlayCommand(cmd);
                case "exit":
                    Console.WriteLine("Game over.");
                    Environment.Exit(0);
                    return true;
                default:
                    Console.WriteLine("\tUnknown command. Type 'help' for a list of commands.");
                    return false;
            }
        }

        private bool ProcessMoveCommand(string[] cmd)
        {
            if (cmd.Length > 1)
            {
                string direction = cmd[1].ToLower();
                if (player.CurrentRoom.Exits.TryGetValue(direction, out Room nextRoom))
                {
                    bool doorIsOpen = player.CurrentRoom.DoorsOpen.TryGetValue(direction, out bool isOpen) && isOpen;
                    if (!doorIsOpen)
                    {
                        Console.WriteLine($"\tThe door to {nextRoom.Name} is locked.");
                        return false;
                    }
                    player.MoveTo(nextRoom);
                    return true;
                }
                Console.WriteLine("\tThere is no exit in that direction.");
                return false;
            }
            Console.WriteLine("\tSpecify a direction to move.");
            return false;
        }

        private bool ProcessTakeCommand(string[] cmd)
        {
            if (cmd.Length > 1)
            {
                string itemName = String.Join(" ", cmd.Skip(1));
                player.TakeItem(itemName);
                return true;
            }
            else
            {
                Console.WriteLine("\tSpecify the item you want to take.");
                return false;
            }
        }

        private bool ProcessUseCommand(string[] cmd)
        {
            if (cmd.Length > 1)
            {
                string itemName = String.Join(" ", cmd.Skip(1));
                player.UseItem(itemName);
                return true;
            }
            else
            {
                Console.WriteLine("\tSpecify the item you want to use.");
                return false;
            }
        }

        private bool ProcessPlayCommand(string[] cmd)
        {
            string gameOverArt = @"
               _____                         ____                 
              / ____|                       / __ \                
             | |  __  __ _ _ __ ___   ___  | |  | |_   _____ _ __ 
             | | |_ |/ _` | '_ ` _ \ / _ \ | |  | \ \ / / _ \ '__|
             | |__| | (_| | | | | | |  __/ | |__| |\ V /  __/ |   
              \_____|\__,_|_| |_| |_|\___|  \____/  \_/ \___|_|   
            ";
            if (cmd.Length > 1 && player.CurrentRoom.Name.ToLower() == "canteen" && cmd[1] == "blackjack")
            {
                Console.WriteLine("\tStarting the Blackjack Game...");
                Item cigarettes = player.Inventory.FirstOrDefault(i => i.Name == "Cigarettes");
                if (cigarettes == null || cigarettes.Quantity == 0)
                {
                    Console.WriteLine("You have no cigarettes to bet.");
                    return false;
                }
                Blackjack game = new Blackjack();
                int finalCigarettes = game.BlackjackRun(cigarettes.Quantity);
                cigarettes.Quantity = finalCigarettes;
                if (cigarettes.Quantity == 0)
                {
                    Console.WriteLine("\n\n\tYou have lost all your cigarettes and cannot continue.");
                    Console.WriteLine(gameOverArt);
                    Environment.Exit(0);
                }
                Console.WriteLine("\tYou returned to the Escape from Prison game!");
                return true;
            }
            else
            {
                Console.WriteLine("\tYou can't play blackjack in this room.");
                return false;
            }
        }

        private bool ProcessLookCommand()
        {
            if (player.CurrentRoom != null)
            {
                player.CurrentRoom.PrintDescription();
                return true;
            }
            else
            {
                Console.WriteLine("\tThere is no room to look around.");
                return false;
            }
        }

        private bool ProcessTalkCommand(string[] cmd)
        {
            if (player.CurrentRoom.Characters.Count == 0)
            {
                Console.WriteLine("There's no one to talk to here.");
                return false;
            }

            if (cmd.Length < 2)
            {
                Console.WriteLine("Specify whom you want to talk to.");
                return false;
            }

            string targetName = String.Join(" ", cmd.Skip(1)).ToLower();
            var character = player.CurrentRoom.Characters.FirstOrDefault(c => c.Name.ToLower() == targetName);
            if (character == null)
            {
                Console.WriteLine("There is no one with that name here.");
                return false;
            }

            switch (player.CurrentRoom.Name.ToLower())
            {
                case "corridor":
                    if (character.Name.ToLower() == "bob")
                    {
                        Console.WriteLine("Do you want to exchange cigarettes for a vodka? (yes/no)");
                        string decision = Console.ReadLine().ToLower().Trim();
                        if (decision == "yes")
                        {
                            return TradeWithInmate(character, player);
                        }
                        else if (decision == "no")
                        {
                            return true;
                        }
                        else
                        {
                            Console.WriteLine("Invalid input.");
                            return false;
                        }
                    }
                    break;
                case "yard":
                    if (character.Name.ToLower() == "guard")
                    {
                        return BribeGuard(character, player);
                    }
                    break;
            }

            Console.WriteLine("You can't talk to them right now.");
            return false;
        }

        private bool TradeWithInmate(Character character, Player player)
        {
            Console.WriteLine($"\tYou talk to {character.Name}. He offers to trade vodka for cigarettes.");
            if (player.Inventory.Any(i => i.Name == "Cigarettes" && i.Quantity >= 30))
            {
                player.RemoveFromInventory("Cigarettes", 30);
                player.AddToInventory(new Item("Vodka", "\tA bottle of vodka, valuable for trading."));
                Console.WriteLine("\tYou traded 30 cigarettes for a bottle of vodka.");
                return true;
            }
            else
            {
                Console.WriteLine("\tYou do not have enough cigarettes. You need at least 30 to make a trade.");
                return false;
            }
        }

        private bool BribeGuard(Character character, Player player)
        {
            if (player.Inventory.Any(i => i.Name == "Vodka" && i.Quantity > 0))
            {
                player.RemoveFromInventory("Vodka", 1);
                player.AddToInventory(new Item("Access Card", "\tA card that opens the gate to freedom.", 1));
                Console.WriteLine($"\tYou give vodka to {character.Name}. In return, he gives you an access card.");
                return true;
            }
            else
            {
                Console.WriteLine("\tYou don't have any vodka to bribe the guard.");
                return false;
            }
        }

        private void PrintHelp()
        {
            Console.WriteLine("\t\tAvailable commands:");
            Console.WriteLine("\t\t'move [direction]' to move to different rooms.");
            Console.WriteLine("\t\t'take [item]' to pick up an item.");
            Console.WriteLine("\t\t'use [item]' to use an item.");
            Console.WriteLine("\t\t'talk [character]' - Talk to a character in the room.");
            Console.WriteLine("\t\t'look' to look around the room.");
            Console.WriteLine("\t\t'inventory' to display your inventory.");
            Console.WriteLine("\t\t'play blackjack' to play a game in the canteen.");
            Console.WriteLine("\t\t'exit' to exit the game.");
        }
    }
}

