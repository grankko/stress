using Microsoft.VisualStudio.TestTools.UnitTesting;
using NuGet.Frameworks;
using Stress.Game.Cards;
using System;
using System.Collections.Generic;
using System.Text;

namespace Stress.Game.Tests
{
    [TestClass]
    public class PlayerTests
    {
        [TestMethod]
        public void CanCreatePlayer()
        {
            var sut = new Player("Anders");
            Assert.IsNotNull(sut);
        }

        [TestMethod]
        public void CanPickUpCards()
        {
            var sut = new Player("Anders");
            var deck = new DeckOfCards();

            Assert.IsNull(sut.OpenCards[0]);
            sut.PickUpCard(deck.DrawCard());
            Assert.IsTrue(sut.Hand.Cards.Count == 0);
            Assert.IsNotNull(sut.OpenCards[0]);

            Assert.IsNull(sut.OpenCards[1]);
            sut.PickUpCard(deck.DrawCard());
            Assert.IsTrue(sut.Hand.Cards.Count == 0);
            Assert.IsNotNull(sut.OpenCards[1]);

            Assert.IsNull(sut.OpenCards[2]);
            sut.PickUpCard(deck.DrawCard());
            Assert.IsTrue(sut.Hand.Cards.Count == 0);
            Assert.IsNotNull(sut.OpenCards[2]);

            Assert.IsNull(sut.OpenCards[3]);
            sut.PickUpCard(deck.DrawCard());
            Assert.IsTrue(sut.Hand.Cards.Count == 0);
            Assert.IsNotNull(sut.OpenCards[3]);

            sut.PickUpCard(deck.DrawCard());
            Assert.IsTrue(sut.Hand.Cards.Count == 1);
        }
    }
}
