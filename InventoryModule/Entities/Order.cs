using InventoryModule.Enums;

namespace InventoryModule.Entities
{
    public class Order
    {
        public int Id { get; set; }
        public string SupplierName { get; set; }
        public DateTime ArrivalDate { get; set; }
        public DateTime OrderDate { get; set; } = DateTime.Now;
        public Status Status { get; set; }

        public Item Item { get; set; }
    }
}
