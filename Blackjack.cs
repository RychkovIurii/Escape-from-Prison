using System;
using System.Collections.Generic;
namespace Prison
{
    public class Blackjack
    {
        PeliKortit peliKortit = new PeliKortit();
        BlackjackAivot blackjackAivot = new BlackjackAivot();

        public int BlackjackRun(int initialCigarettes)
        {
            int cigarettes = initialCigarettes;
            Console.Clear();
            Console.WriteLine("Welcome to the Blackjack Game!");

            peliKortit.ValmistaPakka();
            peliKortit.SekoitettuPakka();
            List<PeliKortti> deck = new List<PeliKortti>(peliKortit.korttiPakka);

            List<KorttiPelaaja> players = SetupPlayers();
            KorttiPelaaja humanPlayer = players.FirstOrDefault(p => p.PelaajaTyyppi == KorttiPelaajatyyppi.Pelaaja);

            humanPlayer.Cigarettes = cigarettes;
            while (true)
            {
                Console.WriteLine($"You have {humanPlayer.Cigarettes} cigarettes.");
                Console.WriteLine("How many cigarettes do you want to bet?");
                string input = Console.ReadLine().Trim().ToLower();
                if (input == "exit")
                {
                    Console.WriteLine("Exiting the game. Thank you for playing!");
                    break;
                }

                if (int.TryParse(input, out int bet) && bet > 0 && bet <= humanPlayer.Cigarettes)
                {
                    humanPlayer.Bet = bet;
                    humanPlayer.Cigarettes -= humanPlayer.Bet;
                }
                else
                {
                    Console.WriteLine("Invalid bet. Please enter a valid number of cigarettes.");
                    continue;
                }

                PlayRound(deck, players);
                Console.WriteLine($"You have {humanPlayer.Cigarettes} cigarettes.");

                Console.WriteLine("Would you like to play another round? Type 'exit' to quit or press Enter to continue.");
                if (Console.ReadLine().Trim().ToLower() == "exit")
                {
                    Console.WriteLine("Game over. Thank you for playing!");
                    break;
                }
                Console.Clear();
            }
            return humanPlayer.Cigarettes;
        }

        private List<KorttiPelaaja> SetupPlayers()
        {
            var players = new List<KorttiPelaaja>
            {
                new KorttiPelaaja { PelaajaTyyppi = KorttiPelaajatyyppi.Pelaaja, Cigarettes = 1 },
                new KorttiPelaaja { PelaajaTyyppi = KorttiPelaajatyyppi.Dealeri, Cigarettes = 100 },
                new KorttiPelaaja { PelaajaTyyppi = KorttiPelaajatyyppi.Jaakko, Cigarettes = 20 }
            };
            return players;
        }

        /*private void MakeAutoBet(KorttiPelaaja npc)
        {
            int bet = new Random().Next(1, Math.Min(5, npc.Cigarettes + 1));
            npc.Bet = bet;
            npc.Cigarettes -= bet;
        }*/

        private void AutoPlayNPC(KorttiPelaaja npc, List<PeliKortti> deck)
        {
            while (true)
            {
                int score = blackjackAivot.BlackjackCounter(npc.korttiKasi);
                if (score >= 17 || deck.Count == 0)
                    break;

                npc.korttiKasi.Add(peliKortit.NostaKortti());
            }
        }

        private void TakeTurn(KorttiPelaaja player, List<PeliKortti> deck)
        {
            bool turnOver = false;
            while (!turnOver)
            {
                Console.WriteLine($"{player.PelaajaTyyppi}'s current hand:");
                player.NaytaKasi();
                int score = blackjackAivot.BlackjackCounter(player.korttiKasi);
                Console.WriteLine($"Current score: {score}");

                if (score >= 21) break;

                Console.WriteLine("Hit or Stand? (h/s)");
                string decision = Console.ReadLine().ToLower().Trim();
                if (decision == "h")
                {
                    player.korttiKasi.Add(peliKortit.NostaKortti());
                }
                else if (decision == "s")
                {
                    turnOver = true;
                }
            }
        }

        private void PlayRound(List<PeliKortti> deck, List<KorttiPelaaja> players)
        {
            foreach (var player in players)
            {
                player.korttiKasi.Clear();
                player.korttiKasi.Add(peliKortit.NostaKortti());
                player.korttiKasi.Add(peliKortit.NostaKortti());
            }

            foreach (var player in players)
            {
                if (player.PelaajaTyyppi != KorttiPelaajatyyppi.Pelaaja)
                {
                    Console.WriteLine($"{player.PelaajaTyyppi}'s current hand:");
                    player.NaytaKasi();
                    int score = blackjackAivot.BlackjackCounter(player.korttiKasi);
                    Console.WriteLine($"Current score: {score}");
                    Console.WriteLine();
                }
            }

            foreach (var player in players)
            {
                if (player.PelaajaTyyppi == KorttiPelaajatyyppi.Pelaaja)
                {
                    TakeTurn(player, deck);
                }
                else
                {
                    AutoPlayNPC(player, deck);
                }
            }

            DetermineRoundOutcome(players);
        }

        private void DetermineRoundOutcome(List<KorttiPelaaja> players)
        {
            KorttiPelaaja dealer = players.First(p => p.PelaajaTyyppi == KorttiPelaajatyyppi.Dealeri);
            int dealerScore = blackjackAivot.BlackjackCounter(dealer.korttiKasi);
            Console.WriteLine($"Dealer's final score: {dealerScore}");
            Console.WriteLine();

            foreach (var player in players.Where(p => p.PelaajaTyyppi != KorttiPelaajatyyppi.Dealeri && p.PelaajaTyyppi != KorttiPelaajatyyppi.Jaakko))
            {
                int playerScore = blackjackAivot.BlackjackCounter(player.korttiKasi);
                Console.WriteLine($"{player.PelaajaTyyppi} final score: {playerScore}");
                Console.WriteLine();

                if ((playerScore > dealerScore && playerScore <= 21) || (dealerScore > 21 && playerScore <= 21))
                {
                    Console.WriteLine($"{player.PelaajaTyyppi} wins!");
                    Console.WriteLine();
                    player.Cigarettes += player.Bet * 2;
                }
                else if (playerScore == dealerScore)
                {
                    Console.WriteLine("It's a draw.");
                    Console.WriteLine();
                    player.Cigarettes += player.Bet;
                }
                else
                {
                    Console.WriteLine($"{player.PelaajaTyyppi} loses.");
                    Console.WriteLine();
                }
            }
        }
    }
}
