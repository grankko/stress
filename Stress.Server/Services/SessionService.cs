using Stress.Server.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Stress.Server.Services
{
    public class SessionService
    {
        public Dictionary<string, GameSession> GameSessions { get; private set; }

        public SessionService()
        {
            GameSessions = new Dictionary<string, GameSession>();
        }

        public string CreateNewGameSession()
        {
            var session = new GameSession(GenerateNewSessionKey());
            GameSessions.Add(session.Key, session);
            return session.Key;
        }

        private string GenerateNewSessionKey()
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
            const string validCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            string keyCandidate = String.Empty;
            Random randomizer = new Random();

            while (keyCandidate.Length < 6)
                keyCandidate += validCharacters[randomizer.Next(0, validCharacters.Length)];

            return keyCandidate;
        }

    }
}
