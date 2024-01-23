namespace InventoryModule.Entities
{
    public class RequestItem
    {
        public int Id { get; set; }
        public int Quantity { get; set; }

        public Item Item { get; set; }
        public Request Request { get; set; }

    }
}