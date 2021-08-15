using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using static CardGame.CommonType;

namespace CardGame.Test
{
    [TestClass]
    public class CardGameTest
    {
        /// <summary>
        /// Check whether new desk have 40 cards or not
        /// </summary>
        [TestMethod]
        public void New_Desk_With_N_Number_Of_Cards()
        {
            int expectedResult = NUMBEROFCARDS;
            CardUtility utility = new CardUtility();
            var actualResult = utility.CreateCards().Count;
            Assert.AreEqual(expectedResult, actualResult);
        }

        /// <summary>
        /// Compare the generated card with shuffled list to verify list is shuffled
        /// </summary>
        [TestMethod]
        public void Check_Shuffle_Functionality()
        {
            CardUtility utility = new CardUtility();
            var cardDesk = utility.CreateCards().ToArray();
            var originalDesk = cardDesk.Select(a => a.CardValue).ToArray();
            utility.Shuffle(cardDesk);
            Assert.IsTrue(string.Join(",", originalDesk) != string.Join(",", cardDesk.Select(a => a.CardValue)));
        }

        /// <summary>
        /// Compare the generated card with shuffled list to verify list is shuffled
        /// </summary>
        [TestMethod]
        public void Shuffle_Discard_Pile_With_DrawPile_If_Draw_Pile_Is_Empty()
        {
            CardUtility utility = new CardUtility();
            var cardDesk = utility.CreateCards().ToArray();
            List<Player> player = new List<Player> {new Player {Name= "Player1",
                                Cards = new Stack(), DiscardPile = new Stack(cardDesk) }};

            var drawPileCount = player.FirstOrDefault().Cards.Count;
            var discardPileCount = player.FirstOrDefault().DiscardPile.Count;

            new CardUtility().ShuffleDiscardPileWithDrawPile(player);

            var shuffledDrawPileCount = player.FirstOrDefault().Cards.Count;
            var shuffledDiscardPileCount = player.FirstOrDefault().DiscardPile.Count;

            Assert.AreEqual(discardPileCount, shuffledDrawPileCount);
            Assert.AreEqual(drawPileCount, shuffledDiscardPileCount);
        }

        /// <summary>
        /// When comparing two cards of the same value, the winner of the next round should win 4 cards  
        /// </summary>
        [TestMethod]
        public void On_Round_Draw_Winner_Of_The_Next_Round_Should_Win_4_Cards()
        {
            int expectedResult = 4;
            CardUtility cardUtility = new CardUtility();
            List<Player> players = new List<Player>();
            Card[] cards = new Card[2];
            cards[0] = new Card((RANK)3);
            cards[1] = new Card((RANK)4);

            players.Add(new Player { Cards = new Stack(cards), Name = "Player1", DiscardPile = new Stack() });

            cards[0] = new Card((RANK)5);
            cards[1] = new Card((RANK)4);
            players.Add(new Player { Cards = new Stack(cards), Name = "Player2", DiscardPile = new Stack() });

            List<Card> cardsOnTable = new List<Card>();
            int numberOfRounds = cardUtility.GetMinimumRoundCount(players);
            for (int round = 0; round < numberOfRounds; round++)
            {
                List<PlayerCard> playerCards = new List<PlayerCard>();

                cardUtility.PopCardFromAllPlayerForSingleRound(players, playerCards);

               cardUtility.CheckRoundWinner(players, playerCards, ref cardsOnTable);
            }
            var player1 = players.Where(a => a.Name == "Player2");
            Assert.AreEqual(expectedResult, player1.Sum(a => a.Cards.Count) + player1.Sum(a => a.DiscardPile.Count));
        }

        /// <summary>
        /// Test conole print of whole game
        /// </summary>
        [TestMethod]
        public void CardGameConolePrint()
        {
            List<Player> players = new List<Player>();
            CardUtility utility = new CardUtility();


            players.Add(new Player { Cards = new Stack(), Name = "Player1", DiscardPile = new Stack() });
            players.Add(new Player { Cards = new Stack(), Name = "Player2", DiscardPile = new Stack() });

            //Create cards
            var cards = utility.CreateCards().ToArray();

            //Shuffle cards
            utility.Shuffle(cards);

            //Distribute card to player
            utility.InitializeCardsInDrawPile(players, cards);

            //Let's play the game
            utility.PlayGame(players, new List<Card>());

        }
    }
}
