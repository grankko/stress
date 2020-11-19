using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Stress.Game;
using Stress.Game.Cards;
using Stress.Server.Services;

namespace Stress.Server.Tests
{
    [TestClass]
    public class GameSessionServiceTests
    {
        private Mock<IGameplay> _gameplayMock;

        [TestInitialize]
        public void InitiateMoq()
        {
            _gameplayMock = new Mock<IGameplay>();
            _gameplayMock.SetupGet(g => g.LeftStack).Returns(new OpenStackOfCards());
            _gameplayMock.SetupGet(g => g.RightStack).Returns(new OpenStackOfCards());
        }

        [TestMethod]
        public void GameStartsWhenTwoPlayersAreAdded()
        {
            var sut = new GameSessionService("asdfgh", _gameplayMock.Object);

            _gameplayMock.SetupGet(g => g.PlayerOne).Returns(new Player("Anders", true));
            var state = sut.AddPlayer("Anders");

            Assert.IsFalse(state.IsReady);

            _gameplayMock.SetupGet(g => g.PlayerTwo).Returns(new Player("Edith", false));
            _gameplayMock.Setup(g => g.CanStart()).Returns(true);
            state = sut.AddPlayer("Edith");

            Assert.IsTrue(state.IsReady);
            _gameplayMock.Verify(g => g.AddPlayer(It.IsAny<string>()), Times.Exactly(2));
        }

        [TestMethod]
        public void DrawIsExecutedWhenConsensusIsReached()
        {
            var sut = new GameSessionService("asdfgh", _gameplayMock.Object);
            var state = sut.PlayerWantsToDraw(1);
            _gameplayMock.Verify(g => g.Draw(), Times.Never);
            Assert.IsFalse(state.DrawExecuted);
            Assert.AreEqual(1, state.DrawRequestedByPlayer);

            state = sut.PlayerWantsToDraw(2);
            _gameplayMock.Verify(g => g.Draw(), Times.Once);
            Assert.IsTrue(state.DrawExecuted);
        }

        [TestMethod]
        public void RematchStartedWhenConsensusIsReached()
        {
            var sut = new GameSessionService("asdfgh", _gameplayMock.Object);
            var state = sut.RequestNewGame(1);
            _gameplayMock.Verify(g => g.RestartGame(), Times.Never);
            Assert.IsFalse(state.RematchStarted);

            state = sut.RequestNewGame(2);
            _gameplayMock.Verify(g => g.RestartGame(), Times.Once);
            Assert.IsTrue(state.RematchStarted);
        }
    }
}
