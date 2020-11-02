using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Stress.Server.Models;
using Stress.Server.Services;

namespace Stress.Server.Controllers
{
    /// <summary>
    /// Delete this? Everything seems to happen through the Hub and HubContext
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class GameController : ControllerBase
    {

        private readonly SessionService _sessionService;

        public GameController(SessionService sessionService)
        {
            _sessionService = sessionService;
        }

        [HttpGet]
        public IEnumerable<GameSession> Get()
        {
            return _sessionService.GameSessions.Values;
        }

        [HttpPost("create")]
        public IActionResult CreateNewSession([FromBody]string playerName)
        {
            var sessionKey = _sessionService.CreateNewGameSession();
            _sessionService.GameSessions[sessionKey].AddPlayer(playerName);
            return Ok(sessionKey);
        }

        [HttpPost("join")]
        public IActionResult JoinSession(JoinGameModel request)
        {

            if (!_sessionService.GameSessions.ContainsKey(request.SessionKey))
                return NotFound();

            var session = _sessionService.GameSessions[request.SessionKey];
            session.AddPlayer(request.PlayerName);
            return Ok();
        }
    }
}
