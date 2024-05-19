using System;
namespace Prison
{
    public class PeliKortit
    {
        public List<PeliKortti> korttiPakka = new List<PeliKortti>();

        public void ValmistaPakka()
        {
            korttiPakka.Clear();
            KorttiMaa referenssiMaa = 0;
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 13; j++)
                {
                    string korttiNimi = "";
                    switch (j)
                    {
                        case 0:
                            korttiNimi = "A";
                            break;
                        case 10:
                            korttiNimi = "J";
                            break;
                        case 11:
                            korttiNimi = "Q";
                            break;
                        case 12:
                            korttiNimi = "K";
                            break;
                        default:
                            korttiNimi = (j + 1).ToString();
                            break;
                    }
                    // ♠ ♥ ♦ ♣
                    switch (referenssiMaa)
                    {
                        case KorttiMaa.Risti:
                            korttiNimi = korttiNimi + "♣";
                            break;
                        case KorttiMaa.Pata:
                            korttiNimi = korttiNimi + "♠";
                            break;
                        case KorttiMaa.Ruutu:
                            korttiNimi = korttiNimi + "♦";
                            break;
                        case KorttiMaa.Hertta:
                            korttiNimi = korttiNimi + "♥";
                            break;
                    }
                    korttiPakka.Add(new PeliKortti() { KorttiNumero = j + 1, Maa = referenssiMaa, KorttiNimi = korttiNimi });
                }
                referenssiMaa++;
            }
        }

        //Palautta listalta yhden stunnaisen esineen ja poistaa sen listalta
        public PeliKortti NostaKortti()
        {
            if (korttiPakka.Count == 0)
            {
                Console.WriteLine("Reshuffling the deck...");
                ValmistaPakka();
                SekoitettuPakka();
            }

            Random rand = new Random();
            int korttiIndex = rand.Next(korttiPakka.Count);
            PeliKortti palautusKortti = korttiPakka[korttiIndex];
            korttiPakka.RemoveAt(korttiIndex);
            return palautusKortti;
        }

        // Sekitta Listan sisàllòn jàrjestyksen
        public void SekoitettuPakka()
        {
            Random rng = new Random();
            int n = korttiPakka.Count;
            while (n > 1)
            {
                int k = rng.Next(n--);
                PeliKortti temp = korttiPakka[k];
                korttiPakka[k] = korttiPakka[n];
                korttiPakka[n] = temp;
            }
        }

        public PeliKortti NostaYlin(List<PeliKortti> pakka)
        {
            PeliKortti palautusKortti = pakka.Last();
            pakka.RemoveAt(pakka.Count - 1);
            return palautusKortti;
        }

        public void ReturnCardsToDeck(List<PeliKortti> usedCards, List<PeliKortti> deck)
        {
            deck.AddRange(usedCards);
            usedCards.Clear();
            SekoitettuPakka();
        }
    }

    public enum KorttiMaa
    {
        Risti = 0,
        Pata = 1,
        Hertta = 2,
        Ruutu = 3
    }
    public class PeliKortti
    {
        public int KorttiNumero { get; set; }
        public KorttiMaa Maa { get; set; }
        public string KorttiNimi { get; set; }

        public void ShowCard()
        {
            Console.Write("[" + KorttiNimi + "]");
        }
        public void ShowCard(bool rivitys)
        {
            Console.Write("[" + KorttiNimi + "]");
            if (rivitys == true)
            {
                Console.WriteLine();
            }
        }
    }

    public enum KorttiPelaajatyyppi
    {
        Pelaaja,
        Dealeri,
        Jaakko
    }

    public class KorttiPelaaja
    {
        public List<PeliKortti> korttiKasi = new List<PeliKortti>();
        public KorttiPelaajatyyppi PelaajaTyyppi { get; set; }
        public int Cigarettes { get; set; }
        public int Bet { get; set; }

        public void NaytaKasi()
        {
            foreach (PeliKortti kortti in korttiKasi)
            {
                kortti.ShowCard(true);
            }
            Console.WriteLine();
        }
    }
}
