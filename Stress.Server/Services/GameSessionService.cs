using Stress.Game;
using Stress.Game.Cards;
using Stress.Server.Models;

namespace Stress.Server.Services
{
    public class GameSessionService : IGameSessionService
    {
        public string Key { get; private set; }

        private readonly IGameplay _gameplay;

        private bool CanGameStart { get => _gameplay.CanStart(); }

        private ConsensusService _drawConsensus = new ConsensusService(2);
        private ConsensusService _rematchConsensus = new ConsensusService(2);

        public GameSessionService(string key, IGameplay gameplay)
        {
            Key = key;
            _gameplay = gameplay;
        }

        public GameState AddPlayer(string nickName)
        {
            _gameplay.AddPlayer(nickName);
            return GetStateOfPlay();
        }

        public GameState PlayerPlaysCardOnStack(int playerNumber, int slotNumber, bool isLeftStack)
        {
            var player = _gameplay.GetPlayerByNumber(playerNumber);

            var stack = _gameplay.LeftStack;
            if (!isLeftStack)
                stack = _gameplay.RightStack;

            Card cardToPlay = player.OpenCards[slotNumber - 1];
            _gameplay.PlayCardOnStack(player, cardToPlay, stack);

            return GetStateOfPlay();
        }

        public GameState PlayerCallsStress(int playerNumber)
        {
            Player loser = _gameplay.PlayerCallsStressEvent(playerNumber);

            var state = GetStateOfPlay();
            state.PlayerOneState.LostStressEvent = loser.IsPlayerOne;
            state.PlayerTwoState.LostStressEvent = !loser.IsPlayerOne;
            return state;
        }

        public GameState RequestNewGame(int playerNumber)
        {
            var consensusReached = _rematchConsensus.SignalAccept(playerNumber);

            if (consensusReached)
                _gameplay.RestartGame();

            var state = GetStateOfPlay();

            // If only one player has signaled interest in a rematch,
            // include that information in the game state to show the other player.
            state.RematchStarted = consensusReached;
            if (!consensusReached)
                state.NewGameRequestedByPlayer = playerNumber;

            return state;
        }

        public GameState PlayerWantsToDraw(int playerNumber)
        {
            var consensusReached = _drawConsensus.SignalAccept(playerNumber);
            if (consensusReached)
                _gameplay.Draw();

            var state = GetStateOfPlay();
            state.DrawExecuted = consensusReached;

            // If only one player has signaled interest in a draw event,
            // include that information in the game state to show the other player.
            if (!consensusReached)
                state.DrawRequestedByPlayer = playerNumber;

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
            state.PlayerOneState = GetStateOfPlayer(_gameplay.PlayerOne);
            state.PlayerTwoState = GetStateOfPlayer(_gameplay.PlayerTwo);
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

        private PlayerState GetStateOfPlayer(Player player)
        {
            var state = new PlayerState();
            state.NickName = player.NickName;
            state.CardsLeft = player.Hand.Cards.Count;
            state.CardSlot1 = player.OpenCards[0]?.ShortName;
            state.CardSlot2 = player.OpenCards[1]?.ShortName;
            state.CardSlot3 = player.OpenCards[2]?.ShortName;
            state.CardSlot4 = player.OpenCards[3]?.ShortName;
            state.PlayerNumber = player.PlayerNumber;
            return state;
        }
    }
}