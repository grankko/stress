using Microsoft.AspNetCore.Mvc;
using Stress.Server.Services;
using System.Collections.Generic;

namespace Stress.Server.Controllers
{
    /// <summary>
    /// Delete this? Everything seems to happen through the Hub and HubContext
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class GameController : ControllerBase
    {

        private readonly SessionManagementService _sessionService;

        public GameController(SessionManagementService sessionService)
        {
            _sessionService = sessionService;
        }

        [HttpGet]
        public IEnumerable<IGameSessionService> Get()
        {
            return _sessionService.GameSessions.Values;
        }
    }
}
