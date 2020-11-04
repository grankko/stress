using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
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
        public IEnumerable<GameSessionService> Get()
        {
            return _sessionService.GameSessions.Values;
        }
    }
}
