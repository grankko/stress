using Microsoft.AspNetCore.SignalR;
using Stress.Game;
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

        public void AddPlayer(string nickName)
        {
            _gameplay.AddPlayer(nickName);
            if (_gameplay.CanStart())
                _hubContext.Clients.Group(Key).SendAsync("gameStateChanged", GetStateOfPlay());
        }

        private GameState GetStateOfPlay()
        {
            // todo: introduce proper models
            var state = new GameState();
            state.PlayerOneState = GetStateOfPlayer(_gameplay.PlayerOne);
            state.PlayerTwoState = GetStateOfPlayer(_gameplay.PlayerTwo);
            state.LeftStackTopCard = _gameplay.LeftStack.TopCard?.ToString();
            state.RightStackTopCard = _gameplay.RightStack.TopCard?.ToString();
            return state;
        }

        private PlayerState GetStateOfPlayer(Player player)
        {
            var state = new PlayerState();
            state.NickName = player.NickName;
            state.CardsLeft = player.Hand.Cards.Count;
            state.CardSlot1 = player.OpenCards[0]?.ToString();
            state.CardSlot2 = player.OpenCards[1]?.ToString();
            state.CardSlot3 = player.OpenCards[2]?.ToString();
            state.CardSlot4 = player.OpenCards[3]?.ToString();
            return state;
        }
    }
}