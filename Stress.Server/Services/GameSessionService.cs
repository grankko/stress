using Stress.Game;
using Stress.Game.Cards;
using Stress.Server.Models;

namespace Stress.Server.Services
{
    public class GameSessionService
    {
        public string Key { get; private set; }
        public bool CanGameStart { get => _gameplay.CanStart(); }

        private Gameplay _gameplay;

        private bool _playerOneWantsToDraw = false;
        private bool _playerTwoWantsToDraw = false;

        private bool _playerOneWantsNewGame = false;
        private bool _playerTwoWantsNewGame = false;

        public GameSessionService(string key)
        {
            Key = key;
            _gameplay = new Gameplay();
        }

        public GameState AddPlayer(string nickName)
        {
            _gameplay.AddPlayer(nickName);
            return GetStateOfPlay();
        }

        public GameState PlayerPlaysCardOnStack(int playerNumber, int slotNumber, bool isLeftStack)
        {
            var player = _gameplay.PlayerOne;
            if (playerNumber == 2)
                player = _gameplay.PlayerTwo;

            var stack = _gameplay.LeftStack;
            if (!isLeftStack)
                stack = _gameplay.RightStack;

            Card cardToPlay = player.OpenCards[slotNumber - 1];
            _gameplay.PlayCardOnStack(player, cardToPlay, stack);

            return GetStateOfPlay();
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

        public GameState PlayerCallsStress(int playerNumber)
        {
            Player loser = null;
            if (playerNumber == 1)
                loser = _gameplay.PlayerCallsStressEvent(_gameplay.PlayerOne); // todo: ugly
            else if (playerNumber == 2)
                loser = _gameplay.PlayerCallsStressEvent(_gameplay.PlayerTwo);

            var state = GetStateOfPlay();
            state.PlayerOneState.LostStressEvent = loser.IsPlayerOne;
            state.PlayerTwoState.LostStressEvent = !loser.IsPlayerOne;
            return state;
        }

        public GameState RequestNewGame(int playerNumber)
        {
            var newGameStarted = false;

            if (playerNumber == 1)
                _playerOneWantsNewGame = true;
            if (playerNumber == 2)
                _playerTwoWantsNewGame = true;

            if (_playerOneWantsNewGame && _playerTwoWantsNewGame)
            {
                _gameplay.RestartGame();
                newGameStarted = true;
                _playerOneWantsNewGame = false;
                _playerTwoWantsNewGame = false;
            }

            var state = GetStateOfPlay();

            state.RematchStarted = newGameStarted;
            if (!newGameStarted)
                state.NewGameRequestedByPlayer = playerNumber;

            return state;
        }

        private GameState GetStateOfPlay()
        {
            var state = new GameState();

            if (!CanGameStart)
            {
                state.IsReady = false;
                return state;
            }

            state.IsReady = true;
            state.PlayerOneState = GetStateOfPlayer(_gameplay.PlayerOne, 1);
            state.PlayerTwoState = GetStateOfPlayer(_gameplay.PlayerTwo, 2);
            state.LeftStackTopCard = _gameplay.LeftStack.TopCard?.ShortName;
            state.LeftStackSize = _gameplay.LeftStack.Cards.Count;
            state.RightStackTopCard = _gameplay.RightStack.TopCard?.ShortName;
            state.RightStackSize = _gameplay.RightStack.Cards.Count;

            if (_gameplay.PlayerOne.HasWon)
                state.WinnerName = _gameplay.PlayerOne.NickName;
            else if (_gameplay.PlayerTwo.HasWon)
                state.WinnerName = _gameplay.PlayerTwo.NickName;

            return state;
        }

        private PlayerState GetStateOfPlayer(Player player, int playerNumber)
        {
            var state = new PlayerState();
            state.NickName = player.NickName;
            state.CardsLeft = player.Hand.Cards.Count;
            state.CardSlot1 = player.OpenCards[0]?.ShortName;
            state.CardSlot2 = player.OpenCards[1]?.ShortName;
            state.CardSlot3 = player.OpenCards[2]?.ShortName;
            state.CardSlot4 = player.OpenCards[3]?.ShortName;
            state.PlayerNumber = playerNumber;
            return state;
        }
    }
}