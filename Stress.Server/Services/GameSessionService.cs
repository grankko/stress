using Microsoft.AspNetCore.SignalR;
using Stress.Game;
using Stress.Game.Cards;
using Stress.Server.Models;
using System;

namespace Stress.Server.Services
{
    public class GameSessionService
    {
        public string Key { get; private set; }
        public bool CanGameStart { get => _gameplay.CanStart(); }

        private Gameplay _gameplay;

        private bool _playerOneWantsToDraw = false;
        private bool _playerTwoWantsToDraw = false;

        public GameSessionService(string key)
        {
            Key = key;
            _gameplay = new Gameplay();
        }

        public void AddPlayer(string nickName)
        {
            _gameplay.AddPlayer(nickName);
        }

        public void PlayerPlaysCardOnStack(int playerNumber, int slotNumber, bool isLeftStack)
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
            var state = new GameState();
            state.PlayerOneState = GetStateOfPlayer(_gameplay.PlayerOne);
            state.PlayerTwoState = GetStateOfPlayer(_gameplay.PlayerTwo);
            state.LeftStackTopCard = _gameplay.LeftStack.TopCard?.ShortName;
            state.RightStackTopCard = _gameplay.RightStack.TopCard?.ShortName;
            return state;
        }

        public GameState PlayerWantsToDraw(int playerNumber)
        {
            var drawExecuted = false;

            if (playerNumber == 1)
                _playerOneWantsToDraw = true;
            if (playerNumber == 2)
                _playerTwoWantsToDraw = true;

            if (_playerOneWantsToDraw && _playerTwoWantsToDraw)
            {
                _gameplay.Draw();
                _playerOneWantsToDraw = false;
                _playerTwoWantsToDraw = false;

                drawExecuted = true;
            }

            var state = GetStateOfPlay();
            state.DrawExecuted = drawExecuted;
            if (!drawExecuted)
                state.DrawRequestedByPlayer = playerNumber;

            return state;
        }

        public void PlayerCallsStress(int playerNumber)
        {
            if (playerNumber == 1)
                _gameplay.PlayerCallsStressEvent(_gameplay.PlayerOne); // todo: ugly
            else if (playerNumber == 2)
                _gameplay.PlayerCallsStressEvent(_gameplay.PlayerTwo);
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