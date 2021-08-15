using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static CardGame.CommonType;

namespace CardGame
{
   public class Card
    {

        public Card(RANK rank)
        {
            CardValue = rank;
        }
        public RANK CardValue { get; set; }
        
    }
}
