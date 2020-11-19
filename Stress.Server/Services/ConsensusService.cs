using System;
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

        /// <summary>
        /// A participants signals that they have accepted. If consensus is reached the state resets.
        /// </summary>
        /// <param name="participantNumber">Identifier of the participant</param>
        /// <returns>bool indicating if consensus is reached or not.</returns>
        public bool SignalAccept(int participantNumber)
        {
            if (participantNumber < 1 || participantNumber > _participants.Length)
                throw new ArgumentException();

            _participants[participantNumber - 1] = true;
            var haveAllAccepted = _participants.All(p => p == true);
            if (haveAllAccepted)
                _participants = new bool[_participants.Length];

            return haveAllAccepted;
        }
    }
}