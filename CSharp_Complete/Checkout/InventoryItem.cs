namespace Checkout
{
    public class InventoryItem
    {
        public string Sku { get; set; }
        public decimal Price { get; set; }

        public InventoryItem(string sku, decimal price)
        {
            Sku = sku;
            Price = price;
        }
    }
}