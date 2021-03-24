using System.Collections.Generic;
using System.Linq;

namespace Checkout
{
    public class Offer
    {
        private readonly string _sku;
        private readonly int _amountYouMustBuy;
        private readonly decimal _discount;

        public Offer(string sku, int amountYouMustBuy, decimal discount)
        {
            _sku = sku;
            _amountYouMustBuy = amountYouMustBuy;
            _discount = discount;
        }

        internal IEnumerable<Discount> GetDiscounts(List<string> basket)
        {
            var itemsInBasket = basket.Count(s => s == _sku);
            var timesDiscountApplies = (itemsInBasket / _amountYouMustBuy);

            var discounts = new List<Discount>();
            for (var i = 0; i < timesDiscountApplies; i++)
            {
                discounts.Add(new Discount() { Amount = _discount });
            }
            return discounts;
        }
    }
}