using System.Linq;

namespace Stress.Server.Services
{
    /// <summary>
    /// Used to reach agreement between participants
    /// </summary>
    public class ConsensusService
    {
        private bool[] _participants;

        public ConsensusService(int numberOfParticipants) => _participants = new bool[numberOfParticipants];

        public bool SignalAccept(int participantNumber)
        {
            _participants[participantNumber-1] = true;
            var haveAllAccepted = _participants.All(p => p == true);
            if (haveAllAccepted)
                _participants = new bool[_participants.Length];

            return haveAllAccepted;
        }
    }
}