using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static CardGame.CommonType;

namespace CardGame
{
    public class CardUtility
    {

        /// <summary>
        /// Shuffle a generic array
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="card"></param>
		public void Shuffle<T>(T[] card)
		{
			Random random = new Random();

			for (int number = card.Length - 1; number > 1; number--)
			{
				int rIndex = random.Next(number + 1);
				T value = card[rIndex];
				card[rIndex] = card[number];
				card[number] = value;
			}
		}


        /// <summary>
        /// Generate cards to play the game
        /// </summary>
        /// <returns></returns>
		public List<Card> CreateCards()
		{
			List<Card> cards = new List<Card>();
			
			//This loop is used for number of card suit
			for (int suit = 0; suit < NUMBEROFCARDSUIT; suit++)
			{
				for (int rank = 1; rank <= (int)RANK.TEN; rank++)
				{
					cards.Add(new Card((RANK)rank));
				}
			}
			return cards;
		}

        /// <summary>
        /// Initilize cards in player's draw pile
        /// </summary>
        /// <param name="players"></param>
        /// <param name="cards"></param>
		public void InitializeCardsInDrawPile(List<Player> players, Card[] cards)
		{
			int index = 0;
			foreach (Player player in players)
			{
				for (int counter = 0; counter < cards.Length / players.Count; counter++)
				{
					player.Cards.Push(cards[index]);
					index++;
				}
			}
		}

        /// <summary>
        /// Shuffle discard pile with draw Pile
        /// </summary>
        /// <param name="players"></param>
		public void ShuffleDiscardPileWithDrawPile(List<Player> players)
		{
			foreach (var player in players)
			{
				if (player.Cards.Count == 0 && player.DiscardPile.Count > 0)
				{
					var cards = player.DiscardPile.ToArray();

					//shuffle the Discard Pile
					Shuffle(cards);

					//assign the shuffled Discard pile to draw pile
					player.Cards = new Stack(cards);

                    //Set empty discard pile
					player.DiscardPile = new Stack();
				}
			}
		}

        /// <summary>
        /// Get Minimum Round Count
        /// </summary>
        /// <param name="players"></param>
        /// <returns></returns>
        public int GetMinimumRoundCount(List<Player> players)
        {
           return players.OrderBy(a => a.Cards.Count).FirstOrDefault().Cards.Count;
        }
        
        public void PopCardFromAllPlayerForSingleRound(List<Player> players, List<PlayerCard> playerCards)
        {
            var numberOfPlayers = 0;
            //This loop is used for n number of players
            //pick the nth value of all player
            while (numberOfPlayers < players.Count)
            {
                playerCards.Add(new PlayerCard
                {
                    Value = (Card)players[numberOfPlayers].Cards.Pop(),
                    PlayerName = players[numberOfPlayers].Name
                });
                numberOfPlayers++;
            }
        }

        /// <summary>
        /// Play the game with n number of player
        /// </summary>
        /// <param name="players">List of player including assigned cards</param>
        /// <param name="cardOnTable">List of cards draw on last round on the table</param>

        public void PlayGame(List<Player> players, List<Card> cardOnTable)
        {
            var rounds = GetMinimumRoundCount(players);
            while (rounds > 0)
            {
                List<PlayerCard> playerCards = new List<PlayerCard>();

                PopCardFromAllPlayerForSingleRound(players, playerCards);

                //Print Player name with card count and current card
                foreach (var player in players)
                {
                    var currentCard = playerCards.OrderByDescending(item => item.PlayerName == player.Name).FirstOrDefault();
                    Console.WriteLine($"{player.Name} ({(player.Cards.Count + 1) + player.DiscardPile.Count}): {(int)currentCard.Value.CardValue}");
                }

                CheckRoundWinner(players, playerCards, ref cardOnTable);

                Console.WriteLine();

                rounds--;
            }

            if (players.Any(a => a.Cards.Count == 0 && a.DiscardPile.Count == 0))
            {
                var winner = players.Select(a=>new {TotalCard = a.Cards.Count + a.DiscardPile.Count, Name = a.Name})
                    .OrderByDescending(b=>b.TotalCard).FirstOrDefault();

                Console.WriteLine($"{winner.Name} wins the game!");
            }
            else
            {
                new CardUtility().ShuffleDiscardPileWithDrawPile(players);

                //recursive call until discard pile get empty
                PlayGame(players, cardOnTable);
            }
        }


        /// <summary>
        /// Check each round winner
        /// </summary>
        /// <param name="players"></param>
        /// <param name="playerCards"></param>
        /// <param name="cardOnTable"></param>
        public void CheckRoundWinner(List<Player> players,  List<PlayerCard> playerCards, ref List<Card> cardOnTable)
        {
            //Get First card value and check with all values to see all are same or not
            var firstRecordValue = playerCards.FirstOrDefault().Value.CardValue;
            if (playerCards.All(x => x.Value.CardValue == firstRecordValue))
            {
                cardOnTable.AddRange(playerCards.Select(a => a.Value));
                Console.WriteLine($"No winner in this round");
            }
            else
            {
                var roundWinner = playerCards.OrderByDescending(item => item.Value.CardValue).FirstOrDefault();

                Console.WriteLine($"{roundWinner.PlayerName}  wins this round");

                //insert played cards into winners discard Pile
                var winnerPlayer = players.Where(a => a.Name == roundWinner.PlayerName).FirstOrDefault();

                foreach (var discardPile in playerCards)
                {
                    winnerPlayer.DiscardPile.Push(discardPile.Value);
                }

                //push previous round(s) draw card value to winner discard Pile
                if (cardOnTable.Count > 0)
                {
                    foreach (var tableCard in cardOnTable)
                    {
                        winnerPlayer.DiscardPile.Push(tableCard);
                    }
                    cardOnTable = new List<Card>();
                }
            }
        }
    }
}
