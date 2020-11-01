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
        public bool PlayOnLeftStack { get; set; }


        public PlayCardMessage(Player player, Card card, bool playOnLeftStack)
        {
            Player = player;
            Card = card;
            PlayOnLeftStack = playOnLeftStack;
        }
    }
}
