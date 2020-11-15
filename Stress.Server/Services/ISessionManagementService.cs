using System.Collections.Generic;

namespace Stress.Server.Services
{
    public interface ISessionManagementService
    {
        Dictionary<string, IGameSessionService> GameSessions { get; }

        string CreateNewGameSession();
        string GenerateNewSessionKey();
    }
}