﻿using Microsoft.AspNetCore.SignalR;
using Stress.Server.Models;
using Stress.Server.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Stress.Server.Hubs
{
    public class StressHub : Hub
    {
        private readonly SessionService _sessionService;

        public StressHub(SessionService sessionService)
        {
            _sessionService = sessionService;
        }

        public async Task<string> CreateGameSession(string nickName)
        {
            var sessionKey = _sessionService.CreateNewGameSession();
            _sessionService.GameSessions[sessionKey].AddPlayer(nickName);
            
            await Groups.AddToGroupAsync(this.Context.ConnectionId, sessionKey);
            await Clients.Group(sessionKey).SendAsync("infoMessage", $"{nickName} joined the game {sessionKey}.");
            return sessionKey;
        }

        public async void ExecutePlayerAction(string sessionKey, int playerNumber, int slotNumber, bool isLeftStack)
        {
            var session = _sessionService.GameSessions[sessionKey];
            session.ExecutePlayerAction(playerNumber, slotNumber, isLeftStack);
            await Clients.Group(sessionKey).SendAsync("gameStateChanged", session.GetStateOfPlay());
        }

        public async void PlayerWantsToDraw(string sessionKey, int playerNumber)
        {
            var session = _sessionService.GameSessions[sessionKey];


            // todo: ugly
            var drawExecuted = session.PlayerWantsToDraw(playerNumber);

            var state = session.GetStateOfPlay();
            state.DrawExecuted = drawExecuted;
            if (!state.DrawExecuted)
                state.DrawRequestedByPlayer = playerNumber;

            await Clients.Group(sessionKey).SendAsync("gameStateChanged", state);
        }

        public async void JoinGameSession(string nickName, string sessionKey)
        {
            if (!_sessionService.GameSessions.ContainsKey(sessionKey))
                throw new HubException($"{sessionKey} does not exist.");

            var session = _sessionService.GameSessions[sessionKey];
            await Groups.AddToGroupAsync(this.Context.ConnectionId, sessionKey);
            session.AddPlayer(nickName);
            await Clients.Group(sessionKey).SendAsync("infoMessage", $"{nickName} joined the game {sessionKey}.");

            if (session.CanGameStart)
                await Clients.Group(sessionKey).SendAsync("gameStateChanged", session.GetStateOfPlay());
        }
    }
}
