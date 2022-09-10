using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS_Homework_08_CardGame
{
    public class Game
    {
        public bool IsRunning;
        string[] names = { "Ace", "Base", "Cali", "Dude", "Eugene", "Fido", "Gary", "Hugo", "Ilya", "Jim", "Kim", "Luis" };
        List<Player> Players;
        int Order;
        Karta[] Deck;
        public string[] Suit = { "Hearts", "Diamonds", "Clubs", "Spades" };
        public int[] Rank = { 6, 7, 8, 9, 10, 11, 12, 13, 14 };
        Queue<Karta> CardsOnTable;

        public Game(int playerCount)
        {
            IsRunning = true;
            Players = new List<Player>(playerCount);
            Order = 0;
            CardsOnTable = new Queue<Karta>(playerCount);
            Deck = new Karta[36];
            MakePlayers();
            CreateDeck();
            ShuffleDeck();
            GiveCards();
        }

        public void MakePlayers()
        {
            for (int i = 0; i < Players.Capacity; i++)
            {
                Players.Add(new Player(names[i]));
                //Players[i] = new Player();
            }
        }
        public void CreateDeck()
        {
            int index = 0;
            string[] rankNames = { "6", "7", "8", "9", "10", "Jack", "Queen", "King", "Ace" };
            for (int i = 0; i < Suit.Length; i++)
            {
                for (int j = 0; j < Rank.Length; j++)
                {
                    Deck[index++] = new Karta(Suit[i], Rank[j], rankNames[j]);
                }
            }
        }
        public void ShuffleDeck()
        {
            Random random = new Random();
            int indexOne;
            int indexTwo;
            Karta temp;
            for (int i = 0; i < 200; i++)
            {
                indexOne = random.Next(0, 36);
                indexTwo = random.Next(0, 36);
                temp = Deck[indexOne];
                Deck[indexOne] = Deck[indexTwo];
                Deck[indexTwo] = temp;
            }
        }
        public void GiveCards()
        {
            int toWho;
            for (int i = 0; i < Deck.Length; i++)
            {
                toWho = i % Players.Count;
                Players[toWho].Hand.Enqueue(Deck[i]);
            }
        }
        public void MakeTurn()
        {
            Console.WriteLine($"--------------------------------------------------------");
            Console.WriteLine($"\tNew Turn. Player \"{Players[Order].Name}\" starts...");

            Karta currentMaxCard = Players[Order].Hand.Peek();
            int leadingPlayer = Order;
            int currPlayer;

            for (int i = 0; i < Players.Count; i++)
            {
                // who lay card now
                currPlayer = (Order + i) % Players.Count;

                if (Players[currPlayer].Hand.Count > 0)
                {
                    // is his card currently the biggest?
                    if (currentMaxCard.Rank < Players[currPlayer].Hand.Peek().Rank)
                    {
                        currentMaxCard = Players[currPlayer].Hand.Peek();
                        leadingPlayer = currPlayer;
                    }
                    // lay card on table
                    Console.WriteLine($"Player {Players[currPlayer].Name} is laying card: {Players[currPlayer].Hand.Peek().RankName} of {Players[currPlayer].Hand.Peek().Suit}");
                    CardsOnTable.Enqueue(Players[currPlayer].Hand.Dequeue());
                }
            }

            // Give cards to leading player
            Console.Write($"\nPlayer {Players[leadingPlayer].Name} takes");
            while (CardsOnTable.Count > 0)
            {
                Console.Write($" | {CardsOnTable.Peek().RankName} of {CardsOnTable.Peek().Suit} | ");
                Players[leadingPlayer].Hand.Enqueue(CardsOnTable.Dequeue());
            }
            Console.WriteLine("\n");

            Console.WriteLine("Current standings:");
            for (int i = 0; i < Players.Count; i++)
            {
                Console.WriteLine($"Player {Players[i].Name} owns {Players[i].Hand.Count} cards");

                // If player owns all cards, declared as winner
                if (Players[i].Hand.Count == 36)
                {
                    Console.WriteLine($"Player {Players[i].Name} Wins the game.");
                    IsRunning = false;
                    Console.ReadLine();
                }

                // Player that own no cards, leaves the game
                if (Players[i].Hand.Count == 0)
                {
                    Console.WriteLine($"Player {Players[i].Name} leaves the game.");
                    Players.RemoveAt(i);
                    Console.ReadLine();
                }
            }
            Console.WriteLine();

            // next turn another player will start first
            Order = (Order + 1) % Players.Count;
        }
    }

    public class Player
    {
        public string Name;
        public Queue<Karta> Hand;
        public Player(string name)
        {
            Name = name;
            Hand = new Queue<Karta>(36);
        }
    }

    public class Karta
    {
        public string Suit;
        public int Rank;
        public string RankName;
        public Karta(string suit, int rank, string rankName)
        {
            Suit = suit;
            Rank = rank;
            RankName = rankName;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Game game = new Game(4);

            while(game.IsRunning)
            {
                game.MakeTurn();
                Console.ReadLine();
            }

            Console.ReadLine();
        }
    }
}
