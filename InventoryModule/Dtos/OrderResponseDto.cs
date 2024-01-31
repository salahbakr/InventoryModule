using InventoryModule.Entities;

namespace InventoryModule.Dtos
{
    public class OrderResponseDto
    {
        public string SupplierName { get; set; }
        public DateTime ArrivalDate { get; set; }
        public DateTime OrderDate { get; set; }

        public ItemResponseDto Item { get; set; }
    }
}
