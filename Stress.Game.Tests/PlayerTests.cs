using Microsoft.VisualStudio.TestTools.UnitTesting;
using Stress.Game.Cards;

namespace Stress.Game.Tests
{
    [TestClass]
    public class PlayerTests
    {
        [TestMethod]
        public void CanCreatePlayer()
        {
            var sut = new Player("Anders", true);
            Assert.IsNotNull(sut);
        }

        [TestMethod]
        public void CanPickUpCards()
        {
            var sut = new Player("Anders", true);
            var deck = StackOfCards.CreateFullDeckOfCards();

            Assert.IsNull(sut.OpenCards[0]);            // First open slot of four is not occupied.
            sut.PickUpCard(deck.DrawCard());            // Player picks up one card.
            Assert.IsNotNull(sut.OpenCards[0]);         // First open slot of four is now occupied.
            Assert.IsTrue(sut.Hand.Cards.Count == 0);   // Closed stack of cards on hand should be the same
                                                        // until all open slots are taken.

            Assert.IsNull(sut.OpenCards[1]);
            sut.PickUpCard(deck.DrawCard());
            Assert.IsNotNull(sut.OpenCards[1]);
            Assert.IsTrue(sut.Hand.Cards.Count == 0);

            Assert.IsNull(sut.OpenCards[2]);
            sut.PickUpCard(deck.DrawCard());
            Assert.IsNotNull(sut.OpenCards[2]);
            Assert.IsTrue(sut.Hand.Cards.Count == 0);

            Assert.IsNull(sut.OpenCards[3]);
            sut.PickUpCard(deck.DrawCard());
            Assert.IsNotNull(sut.OpenCards[3]);
            Assert.IsTrue(sut.Hand.Cards.Count == 0);

            sut.PickUpCard(deck.DrawCard());
            Assert.IsTrue(sut.Hand.Cards.Count == 1);   // All open slots taken. Card should go to closed stack.
        }
    }
}
