using Microsoft.AspNetCore.SignalR;
using Stress.Server.Services;
using System.Threading.Tasks;

namespace Stress.Server.Hubs
{
    public class StressHub : Hub
    {
        private const string GameStateChangedMethod = "gameStateChanged";
        private readonly ISessionManagementService _sessionService;

        public StressHub(ISessionManagementService sessionService)
        {
            _sessionService = sessionService;
        }

        public async Task<string> CreateGameSession(string nickName)
        {
            var sessionKey = _sessionService.CreateNewGameSession();
            _sessionService.GameSessions[sessionKey].AddPlayer(nickName);

            await Groups.AddToGroupAsync(this.Context.ConnectionId, sessionKey);
            return sessionKey;
        }

        public async void JoinGameSession(string nickName, string sessionKey)
        {
            if (!_sessionService.GameSessions.ContainsKey(sessionKey))
                throw new HubException($"{sessionKey} does not exist.");

            var session = _sessionService.GameSessions[sessionKey];
            await Groups.AddToGroupAsync(this.Context.ConnectionId, sessionKey);
            var gameState = session.AddPlayer(nickName);

            // Signal clients to start if both players has joined
            if (gameState.IsReady)
                await Clients.Group(sessionKey).SendAsync(GameStateChangedMethod, gameState);
        }

        public async void PlayerPlaysCardOnStack(string sessionKey, int playerNumber, int slotNumber, bool isLeftStack)
        {
            var session = _sessionService.GameSessions[sessionKey];
            var gameState = session.PlayerPlaysCardOnStack(playerNumber, slotNumber, isLeftStack);
            await Clients.Group(sessionKey).SendAsync(GameStateChangedMethod, gameState);
        }

        public async void PlayerWantsToDraw(string sessionKey, int playerNumber)
        {
            var session = _sessionService.GameSessions[sessionKey];

            var gameState = session.PlayerWantsToDraw(playerNumber);
            await Clients.Group(sessionKey).SendAsync(GameStateChangedMethod, gameState);
        }

        public async void PlayerCallsStress(string sessionKey, int playerNumber)
        {
            var session = _sessionService.GameSessions[sessionKey];
            var gameState = session.PlayerCallsStress(playerNumber);

            await Clients.Group(sessionKey).SendAsync(GameStateChangedMethod, gameState);
        }

        public async void RequestNewGame(string sessionKey, int playerNumber)
        {
            var session = _sessionService.GameSessions[sessionKey];
            var gameState = session.RequestNewGame(playerNumber);
            await Clients.Group(sessionKey).SendAsync(GameStateChangedMethod, gameState);
        }
    }
}
