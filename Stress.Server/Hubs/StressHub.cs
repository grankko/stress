using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using Stress.Server.Services;
using System.Threading.Tasks;

namespace Stress.Server.Hubs
{
    public class StressHub : Hub
    {
        private const string GameStateChangedMethod = "gameStateChanged";
        private readonly ISessionManagementService _sessionService;
        private readonly ILogger<StressHub> _logger;

        public StressHub(ISessionManagementService sessionService, ILogger<StressHub> logger)
        {
            _sessionService = sessionService;
            _logger = logger;
        }

        public async Task<string> CreateGameSession(string nickName)
        {
            var sessionKey = _sessionService.CreateNewGameSession();
            _sessionService.GameSessions[sessionKey].AddPlayer(nickName);

            _logger.LogInformation("STRESSEVENT: Game session created by '{0}'. Key '{1}'.", nickName, sessionKey);

            await Groups.AddToGroupAsync(this.Context.ConnectionId, sessionKey);
            return sessionKey;
        }

        public async void JoinGameSession(string nickName, string sessionKey)
        {
            if (!_sessionService.GameSessions.ContainsKey(sessionKey))
            {
                _logger.LogInformation("STRESSEVENT: Player '{0}' tried to join non existing game '{1}'.", nickName, sessionKey);
                return;
            }                

            var session = _sessionService.GameSessions[sessionKey];
            await Groups.AddToGroupAsync(this.Context.ConnectionId, sessionKey);
            var gameState = session.AddPlayer(nickName);

            _logger.LogInformation("STRESSEVENT: Player '{0}' joined game '{1}'.", nickName, sessionKey);

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
