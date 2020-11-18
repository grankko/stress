using Microsoft.VisualStudio.TestTools.UnitTesting;
using Stress.Server.Services;
using System;

namespace Stress.Server.Tests
{
    [TestClass]
    public class ConsensusServiceTests
    {
        [TestMethod]
        public void CanReachConsensus()
        {
            var sut = new ConsensusService(3);
            Assert.IsFalse(sut.SignalAccept(1));
            Assert.IsFalse(sut.SignalAccept(2));
            Assert.IsTrue(sut.SignalAccept(3));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void CantSignalAcceptOutOfBounds()
        {
            var sut = new ConsensusService(3);
            Assert.IsFalse(sut.SignalAccept(4));
        }
    }
}
