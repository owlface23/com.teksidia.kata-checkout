using System.Linq;
using NUnit.Framework;

namespace Checkout.UnitTests
{
    [TestFixture]
    public class OfferTests
    {
        [TestCase("A", 0, 0)]
        [TestCase("A,A,A", -20, 1)]
        [TestCase("A,A,A,B", -20, 1)]
        [TestCase("A,A,A,A,A,A", -40, 2)]
        [TestCase("A,A,A,A,A,A,A", -40, 2)]
        public void GetDiscounts_GivenBasket_ReturnsApplicableDiscount(string basket, decimal expectedTotalDiscount, int discountsOffered)
        {
            var offer = new Offer("A", 3, -20);
            var discounts = offer.GetDiscounts(basket.Split(',').ToList());
            Assert.AreEqual(expectedTotalDiscount, discounts.Select(d => d.Amount).Sum());
            Assert.AreEqual(discountsOffered, discounts.Count());
        }
    }
}
