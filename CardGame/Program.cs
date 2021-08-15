using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static CardGame.CommonType;

namespace CardGame
{
    class Program
    {
        static void Main(string[] args)
        {

            Console.WriteLine("*************************Game Started****************************************");
            Console.WriteLine("********Please make sure you are entering valid interger number of player");
            Console.WriteLine("************Please enter the different name of players***********");
            Console.WriteLine("************Prss enter after entering the value***********");
            Console.WriteLine("**************************************************");

            //"Belwo features also can be added with minimal change");
            //"1.Cards now have suits(Clubs, Spades, Hearts, Diamonds)- We need to Add one property Suit type in Card DTO")
            // The game has more than 2 players-> Already implemented, we just need to specify number of players
            // Deck size is changed-> As of now its 4*10, its is a configurable thing, we just need to increase RANK enum values



            Console.WriteLine();
            Console.Write("Please enter the number of player:- ");
            int noOfPlayers = Convert.ToInt32(Console.ReadLine());
            if (noOfPlayers < 2)
            {
                Console.WriteLine("Atleast two players are required");
                return;
            }

            //Input name of each player
            List<Player> players = new List<Player>();
            while (noOfPlayers > 0)
            {
                Console.Write("Enter player name:- ");
                players.Add(new Player { Name = Console.ReadLine(), Cards = new Stack(), DiscardPile = new Stack() });
                noOfPlayers--;
            }


            CardUtility utility = new CardUtility();

            //Create cards
            var cards = utility.CreateCards().ToArray();


            //Shuffle cards
            utility.Shuffle(cards);


            //Distribute card to player
            utility.InitializeCardsInDrawPile(players, cards);


            //Let's play the game
            utility.PlayGame(players, new List<Card>());


            //To stop the output in console window
            Console.ReadLine();

        }

    }
}
