using GalaSoft.MvvmLight.Messaging;
using Stress.Game;
using Stress.Game.Cards;
using System;
using System.Collections.Generic;
using System.Text;

namespace Stress.Clients.Desktop.Messages
{
    public class PlayCardMessage : MessageBase
    {
        public Player Player { get; set; }
        public Card Card { get; set; }
        public bool OnLeftPile { get; set; }


        public PlayCardMessage(Player player, Card card, bool onLeftPile)
        {
            Player = player;
            Card = card;
            OnLeftPile = onLeftPile;
        }
    }
}
