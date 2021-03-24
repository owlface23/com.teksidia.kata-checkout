using System.Collections.Generic;
using System.Linq;

namespace Checkout
{
    public class CheckOut
    {
        private readonly List<string> _basket;
        private readonly IList<InventoryItem> _inventory;
        private readonly IList<Offer> _offers;

        public CheckOut(IList<InventoryItem> inventory, IList<Offer> offers)
        {
            _basket = new List<string>();
            _inventory = inventory;
            _offers = offers;
        }

        public void Scan(string product)
        {
            _basket.Add(product);
        }

        public decimal GetTotal()
        {
            var preDiscountTotal = GetPreDiscountTotal();
            var discounts = ApplyOfferDiscounts();
            return preDiscountTotal + discounts;
        }

        private decimal GetPreDiscountTotal()
        {
            return _basket.Sum(item => _inventory.First(i => i.Sku == item).Price);
        }

        private decimal ApplyOfferDiscounts()
        {
            return _offers.Sum(offer => offer.GetDiscounts(_basket).Select(d => d.Amount).Sum());
        }
    }
}