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
        Player[] Players;
        int Order;
        Karta[] Deck;
        public string[] Suit = { "Hearts", "Diamonds", "Clubs", "Spades" };
        public int[] Rank = { 6, 7, 8, 9, 10, 11, 12, 13, 14 };
        Queue<Karta> CardsOnTable;

        public Game(int playerCount)
        {
            Players = new Player[playerCount];
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
            for (int i = 0; i < Players.Length; i++)
            {
                Players[i] = new Player();
            }
        }
        public void CreateDeck()
        {
            int index = 0;
            for (int i = 0; i < Suit.Length; i++)
            {
                for (int j = 0; j < Rank.Length; j++)
                {
                    Deck[index++] = new Karta(Suit[i], Rank[j]);
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
                toWho = i % Players.Length;
                Players[toWho].Hand.Enqueue(Deck[i]);
            }
        }
        public void MakeTurn()
        {
            Console.WriteLine($"\tNew Turn. Player {Order + 1} starts...");
            Karta currentMaxCard = Players[Order].Hand.Peek();
            int leadingPlayer = Order;
            int currPlayer;

            for (int i = 0; i < Players.Length; i++)
            {
                // who lay card now
                currPlayer = (Order + i) % Players.Length;

                if (Players[currPlayer].Hand.Count > 0)
                {
                    // is his card currently the biggest
                    if (currentMaxCard.Rank < Players[currPlayer].Hand.Peek().Rank)
                    {
                        currentMaxCard = Players[currPlayer].Hand.Peek();
                        leadingPlayer = currPlayer;
                    }
                    // lay card on table
                    Console.WriteLine($"Player {currPlayer + 1} is laying card: {Players[currPlayer].Hand.Peek().Rank} of {Players[currPlayer].Hand.Peek().Suit}");
                    CardsOnTable.Enqueue(Players[currPlayer].Hand.Dequeue());
                }
            }

            // Give cards to leading player
            Console.Write($"\nPlayer {leadingPlayer + 1} takes");
            while (CardsOnTable.Count > 0)
            {
                Console.Write($" | {CardsOnTable.Peek().Rank} of {CardsOnTable.Peek().Suit} | ");
                Players[leadingPlayer].Hand.Enqueue(CardsOnTable.Dequeue());
            }
            Console.WriteLine("\n");

            Console.WriteLine("Current standings:");
            for (int i = 0; i < Players.Length; i++)
            {
                Console.WriteLine($"Player {i + 1} owns {Players[i].Hand.Count} cards");
            }
            Console.WriteLine("\n");

            // next turn another player will start first
            Order = (Order + 1) % Players.Length;
        }
    }

    public class Player
    {
        public Queue<Karta> Hand;
        public Player()
        {
            Hand = new Queue<Karta>(36);
        }
    }

    public class Karta
    {
        public string Suit;
        public int Rank;
        public Karta(string suit, int rank)
        {
            Suit = suit;
            Rank = rank;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Game game = new Game(4);
            game.MakeTurn();
            game.MakeTurn();
            game.MakeTurn();
            game.MakeTurn();

            Console.ReadLine();
        }
    }
}
