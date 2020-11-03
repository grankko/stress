using Microsoft.AspNetCore.SignalR;
using Stress.Game;
using Stress.Game.Cards;
using Stress.Server.Hubs;
using System;

namespace Stress.Server.Models
{
    public class GameSession
    {
        private IHubContext<StressHub> _hubContext;  // consider: should the GameSession really speak directly with the hub?

        public string Key { get; private set; }
        private Gameplay _gameplay;

        public GameSession(string key, IHubContext<StressHub> hubContext)
        {
            Key = key;
            _gameplay = new Gameplay();
            _hubContext = hubContext;
        }

        public async void AddPlayer(string nickName)
        {
            _gameplay.AddPlayer(nickName);
            if (_gameplay.CanStart())
                await _hubContext.Clients.Group(Key).SendAsync("gameStateChanged", GetStateOfPlay());
        }

        public void ExecutePlayerAction(int playerNumber, int slotNumber, bool isLeftStack)
        {
            var player = _gameplay.PlayerOne;
            if (playerNumber == 2)
                player = _gameplay.PlayerTwo;

            var stack = _gameplay.LeftStack;
            if (!isLeftStack)
                stack = _gameplay.RightStack;

            Card cardToPlay = player.OpenCards[slotNumber - 1];
            _gameplay.PlayCardOnStack(player, cardToPlay, stack);

        }

        public GameState GetStateOfPlay()
        {
            // todo: introduce proper models
            var state = new GameState();
            state.PlayerOneState = GetStateOfPlayer(_gameplay.PlayerOne);
            state.PlayerTwoState = GetStateOfPlayer(_gameplay.PlayerTwo);
            state.LeftStackTopCard = _gameplay.LeftStack.TopCard?.ShortName;
            state.RightStackTopCard = _gameplay.RightStack.TopCard?.ShortName;
            return state;
        }

        private PlayerState GetStateOfPlayer(Player player)
        {
            var state = new PlayerState();
            state.NickName = player.NickName;
            state.CardsLeft = player.Hand.Cards.Count;
            state.CardSlot1 = player.OpenCards[0]?.ShortName;
            state.CardSlot2 = player.OpenCards[1]?.ShortName;
            state.CardSlot3 = player.OpenCards[2]?.ShortName;
            state.CardSlot4 = player.OpenCards[3]?.ShortName;
            return state;
        }
    }
}