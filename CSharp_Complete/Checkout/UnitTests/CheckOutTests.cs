using System.Collections.Generic;
using NUnit.Framework;

namespace Checkout.UnitTests
{
    [TestFixture]
    public class CheckOutTests
    {
        private List<InventoryItem> _inventory;
        private List<Offer> _offers;

        [SetUp]
        public void SetUp()
        {
            _inventory = new List<InventoryItem>
            {
                new InventoryItem("A", 50M),
                new InventoryItem("B", 30M),
                new InventoryItem("C", 20M),
                new InventoryItem("D", 15M)
            };

            _offers = new List<Offer>()
            {
                new Offer("A", 3, -20),
                new Offer("B", 2, -15)
            };
        }

        [TestCase("A", 50)]
        [TestCase("A,A", 100)]
        [TestCase("A,B", 80)]
        [TestCase("A,A,A", 130)]
        [TestCase("B", 30)]
        [TestCase("B,B", 45)]
        [TestCase("A,B,C,D", 115)]
        [TestCase("A,A,A,A,B,B,B", 255)]
        public void GetTotal_GivenSuccessionOfScannedItems_ReturnsCorrectTotal(string basket, decimal expectedTotal)
        {
            var checkout = new CheckOut(_inventory, _offers);

            foreach (var item in basket.Split(','))
            {
                checkout.Scan(item);
            }
            var result = checkout.GetTotal();
            Assert.AreEqual(expectedTotal, result);
        }


    }
}
