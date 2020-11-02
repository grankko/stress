using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Stress.Server.Models
{
    // todo: delete? why use model, just send to parameters to the Hub
    public class JoinGameModel
    {
        public string SessionKey { get; set; }
        public string PlayerName { get; set; }
    }
}
