using InventoryModule.Entities;

namespace InventoryModule.Dtos
{
    public class RequestItemResponseDto
    {
        public int Id { get; set; }
        public int Quantity { get; set; }

        public ItemResponseDto Item { get; set; }
    }
}
