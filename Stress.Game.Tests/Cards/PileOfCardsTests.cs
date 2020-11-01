using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace Stress.Game.Cards.Tests
{
    [TestClass]
    public class PileOfCardsTests
    {
        [TestMethod]
        public void CanCreateDeck()
        {
            var sut = new DeckOfCards();
            Assert.IsNotNull(sut);
            Assert.AreEqual(sut.Cards.Count, 52);
        }

        [TestMethod]
        public void CanShuffleDeck()
        {
            var sut = new DeckOfCards();
            sut.Shuffle();

            Assert.AreEqual(sut.Cards.Count, 52);
        }
    }
}
