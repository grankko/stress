using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;

namespace Stress.Server.Services
{
    public class SessionManagementService : ISessionManagementService
    {
        public Dictionary<string, IGameSessionService> GameSessions { get; private set; }

        public SessionManagementService()
        {
            GameSessions = new Dictionary<string, IGameSessionService>();
        }

        public string CreateNewGameSession()
        {
            var session = Program.ServiceProvider.GetRequiredService<IGameSessionService>();
            GameSessions.Add(session.Key, session);
            return session.Key;
        }

        public string GenerateNewSessionKey()
        {
            var candidateKey = GenerateCandidateKey();
            var attempts = 0;
            while (GameSessions.ContainsKey(candidateKey))
            {
                attempts++;
                candidateKey = GenerateCandidateKey();

                if (attempts > 100)
                    throw new InvalidOperationException("Failed to generate a new candidate key.");
            }

            return candidateKey;
        }

        private string GenerateCandidateKey()
        {
            const string validCharacters = "abcdefghijkmnopqrstuvwxyzABCDEFGHJKLMNOPQRSTUVWXYZ123456789";
            string keyCandidate = String.Empty;
            Random randomizer = new Random();

            while (keyCandidate.Length < 6)
                keyCandidate += validCharacters[randomizer.Next(0, validCharacters.Length)];

            return keyCandidate;
        }

    }
}
