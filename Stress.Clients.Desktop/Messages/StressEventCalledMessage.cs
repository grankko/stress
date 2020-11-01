using GalaSoft.MvvmLight.Messaging;
using Stress.Game;
using System;
using System.Collections.Generic;
using System.Text;

namespace Stress.Clients.Desktop.Messages
{
    public class StressEventCalledMessage : MessageBase
    {
        public Player CallingPlayer { get; private set; }

        public StressEventCalledMessage(Player player)
        {
            CallingPlayer = player;
        }
    }
}
