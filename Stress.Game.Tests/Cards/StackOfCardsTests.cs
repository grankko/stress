using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Stress.Game.Cards.Tests
{
    [TestClass]
    public class StackOfCardsTests
    {
        [TestMethod]
        public void CanCreateDeck()
        {
            var sut = StackOfCards.CreateFullDeckOfCards();
            Assert.IsNotNull(sut);
            Assert.AreEqual(sut.Cards.Count, 52);
        }

        [TestMethod]
        public void CanShuffleDeck()
        {
            var sut = StackOfCards.CreateFullDeckOfCards();
            sut.Shuffle();

            Assert.AreEqual(sut.Cards.Count, 52);
        }
    }
}
