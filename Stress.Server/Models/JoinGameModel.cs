using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Stress.Server.Models
{
    public class JoinGameModel
    {
        public string SessionKey { get; set; }
        public string PlayerName { get; set; }
    }
}
