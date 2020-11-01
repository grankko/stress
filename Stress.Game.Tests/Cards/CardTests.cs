using Microsoft.VisualStudio.TestTools.UnitTesting;
using Stress.Game;

namespace Stress.Game.Cards.Tests
{
    [TestClass]
    public class CardTests
    {
        [TestMethod]
        public void CanCreateCard()
        {
            var sut = new Card(14, Suit.Spades);
            Assert.IsNotNull(sut);
            Assert.IsTrue(sut.ToString() == "Ace of Spades");
        }

        [TestMethod]
        public void CanCompareCards()
        {
            Card aceOfSpades1 = new Card(14, Suit.Spades);
            Card aceOfSpades2 = new Card(14, Suit.Spades);

            Assert.IsTrue(aceOfSpades1.Equals(aceOfSpades2));

            Card kingOfHearts = new Card(13, Suit.Hearts);

            Assert.IsFalse(kingOfHearts.Equals(aceOfSpades1));
        }
    }
}
