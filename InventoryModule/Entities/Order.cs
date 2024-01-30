namespace InventoryModule.Entities
{
    public class Order
    {
        public int Id { get; set; }
        public string SupplierName { get; set; }
        public DateTime ArrivalDate { get; set; }
        public DateTime OrderDate { get; set; } = DateTime.Now;

        public ICollection<Item> Items { get; set; }
    }
}
