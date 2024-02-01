using InventoryModule.Entities;

namespace InventoryModule.Dtos
{
    public class ItemResponseDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Quantity { get; set; }
        public int OrderQuantity { get; set; }
        public int OrderPoint { get; set; }
        public DateTime DateEntered { get; set; }

        public CategoryResponseDto Category { get; set; }
        public ShelfResponseDto Shelf { get; set; }
    }
}
