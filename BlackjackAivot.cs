using System;
using System.Collections.Generic;
namespace Prison
{
    public class BlackjackAivot
    {
        public int BlackjackCounter(List<PeliKortti> hand)
        {
            int score = 0;
            int aces = 0;

            foreach (var card in hand)
            {
                if (card.KorttiNumero == 1)
                {
                    aces++;
                    score += 11;
                }
                else if (card.KorttiNumero >= 10)
                {
                    score += 10;
                }
                else
                {
                    score += card.KorttiNumero;
                }
            }

            while (score > 21 && aces > 0)
            {
                score -= 10;
                aces--;
            }

            return score;
        }
    }
    /*
    public class BlackjackVisualizer
    {
        BlackjackAivot rules = new BlackjackAivot();

        public void VisualizeBlackjack(List<KorttiPelaaja> players, bool playerPassed)
        {
            Console.Clear();
            foreach (KorttiPelaaja player in players)
            {
                int playerScore = rules.BlackjackCounter(player.korttiKasi);
                string playerName = "";
                switch (player.PelaajaTyyppi)
                {
                    case KorttiPelaajatyyppi.Pelaaja:
                        playerName = "Player";
                        break;
                    case KorttiPelaajatyyppi.Dealeri:
                        playerName = "Dealer";
                        if (playerPassed == false)
                        {
                            Console.Write("Dealer's hand: ");
                            for (int i = 0; i < players.Count; i++)
                            {
                                if (i == 0)
                                {
                                    player.korttiKasi[0].ShowCard();
                                }
                                else
                                {
                                    Console.WriteLine("[ }");
                                }
                                Console.WriteLine();
                            }
                        }
                        break;
                }
                if (player.PelaajaTyyppi != KorttiPelaajatyyppi.Dealeri || playerPassed == true)
                {
                    Console.Write(playerName + "'s hand: ");
                    player.NaytaKasi();
                    Console.Write(playerName + " Score: " + playerScore);
                    if (playerScore > 21)
                    {
                        Console.Write("Bust");
                    }
                    else if (playerScore == 21)
                    {
                        Console.Write("Blackjack");
                    }
                    Console.WriteLine();
                }
                Console.WriteLine("____________________");
            }
            Console.WriteLine();
        }
    }*/
}
