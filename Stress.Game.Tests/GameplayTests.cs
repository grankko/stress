using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

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

            Assert.IsTrue(sut.LeftPile.Cards.Count == 0);
            Assert.IsTrue(sut.RightPile.Cards.Count == 0);

            sut.Draw();

            Assert.IsTrue(sut.LeftPile.Cards.Count == 1);
            Assert.IsTrue(sut.RightPile.Cards.Count == 1);
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
