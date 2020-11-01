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

            Assert.IsNull(sut.CardSlot1);
            sut.PickUpCard(deck.DrawCard());
            Assert.IsTrue(sut.Hand.Cards.Count == 0);
            Assert.IsNotNull(sut.CardSlot1);

            Assert.IsNull(sut.CardSlot2);
            sut.PickUpCard(deck.DrawCard());
            Assert.IsTrue(sut.Hand.Cards.Count == 0);
            Assert.IsNotNull(sut.CardSlot2);

            Assert.IsNull(sut.CardSlot3);
            sut.PickUpCard(deck.DrawCard());
            Assert.IsTrue(sut.Hand.Cards.Count == 0);
            Assert.IsNotNull(sut.CardSlot3);

            Assert.IsNull(sut.CardSlot4);
            sut.PickUpCard(deck.DrawCard());
            Assert.IsTrue(sut.Hand.Cards.Count == 0);
            Assert.IsNotNull(sut.CardSlot4);

            sut.PickUpCard(deck.DrawCard());
            Assert.IsTrue(sut.Hand.Cards.Count == 1);
        }
    }
}
