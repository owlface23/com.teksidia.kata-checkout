using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Xunit;

namespace Checkout1
{
    public class CheckoutTests
    {
        [Theory]
        [InlineData("A99", 50)]
        [InlineData("B15", 30)]
        public void GetsCorrectTotalForSingleItem(string sku, decimal price)
        {
            var checkout = new Checkout(new Catalog());
            checkout.Scan(sku);
            Assert.Equal(price, checkout.Total());
        }

        [Fact]
        public void GetsCorrectTotalForMultipleItems()
        {
            var checkout = new Checkout(new Catalog());
            checkout.Scan("A99");
            checkout.Scan("B15");
            Assert.Equal(80, checkout.Total());
        }
    }

    public class CatalogTests
    {
        [Theory]
        [InlineData("A99", 1, 50)]
        [InlineData("A99", 2, 100)]
        [InlineData("A99", 3, 120)]
        [InlineData("A99", 4, 170)]
        [InlineData("A99", 5, 220)]
        [InlineData("A99", 6, 240)]
        [InlineData("B15", 1, 30)]
        [InlineData("B15", 2, 45)]
        [InlineData("B15", 3, 75)]
        public void ItReturnsCorrectPrice(string sku, int quantity, decimal expectedPrice)
        {
            var catalog = new Catalog();
            var price = catalog.GetPrice(sku, quantity);
            Assert.Equal(expectedPrice, price);
        }
    }

    internal class Checkout
    {
        private Catalog _catalog;
        private Basket _basket = new();

        public Checkout(Catalog catalog)
        {
            _catalog = catalog;
        }

        internal void Scan(string sku)
        {
            if (!_basket.Contains(sku))
            {
                _basket.Add(new BasketItem(sku));
                return;
            }
            _basket[sku].Quantity += 1;
        }

        internal decimal Total()
        {
            var total = 0M;
            foreach (var product in _basket)
            {
                total += _catalog.GetPrice(product.Sku, product.Quantity);
            }
            return total;
        }
    }

    internal class Catalog
    {
        private List<CatalogItem> _items;

        public Catalog()
        {
            _items = new List<CatalogItem>
            {
                new CatalogItem("A99", 50, 3, 120),
                new CatalogItem("B15", 30, 2, 45)
            };
        }

        internal decimal GetPrice(string sku, int quantity)
        {
            var item = _items.FirstOrDefault(_ => _.Sku == sku);

            var offerMultiplier = quantity / item.OfferQuantity;
            var standardMultipler = quantity % item.OfferQuantity;

            return (offerMultiplier * item.OfferPrice) + (standardMultipler * item.Price);
        }
    }

    internal sealed class CatalogItem
    {
        public CatalogItem(string sku, decimal price, int offerQuantity, decimal offerPrice)
        {
            Sku = sku;
            Price = price;
            OfferPrice = offerPrice;
            OfferQuantity = offerQuantity;
        }
        public string Sku { get; }
        public decimal Price { get; }
        public int OfferQuantity { get; }
        public decimal OfferPrice { get; }
    }

    internal sealed class Basket : KeyedCollection<string, BasketItem>
    {
        protected override string GetKeyForItem(BasketItem item)
        {
            return item.Sku;
        }
    }

    internal sealed class BasketItem
    {
        public BasketItem(string sku)
        {
            Sku = sku;
            Quantity = 1;
        }
        public string Sku { get; } // immutable
        public int Quantity { get; set; }
    }
}
