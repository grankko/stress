using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Stress.Game.Tests
{
    [TestClass]
    public class GameplayTests
    {

        [TestMethod]
        public void CanCreateGameplay()
        {
            var sut = new Gameplay();
            Assert.IsNotNull(sut);
            Assert.IsTrue(sut.Deck.Cards.Count == 52);
        }

        [TestMethod]
        public void CanAddPlayers()
        {
            var sut = new Gameplay();
            sut.AddPlayer("Anders");
            sut.AddPlayer("Edith");

            Assert.AreEqual(sut.PlayerOne.NickName, "Anders");
            Assert.AreEqual(sut.PlayerTwo.NickName, "Edith");
        }

        [TestMethod]
        public void CanDoFirstDraw()
        {
            var sut = new Gameplay();
            sut.AddPlayer("Anders");
            sut.AddPlayer("Edith");

            Assert.IsTrue(sut.LeftStack.Cards.Count == 0);
            Assert.IsTrue(sut.RightStack.Cards.Count == 0);

            sut.Draw();

            Assert.IsTrue(sut.LeftStack.Cards.Count == 1);
            Assert.IsTrue(sut.RightStack.Cards.Count == 1);
        }
        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void CantAddThreePlayers()
        {
            var sut = new Gameplay();
            sut.AddPlayer("Anders");
            sut.AddPlayer("Edith");
            sut.AddPlayer("Alice");
        }
    }
}
