namespace InventoryModule.Dtos
{
    public class CreateItemDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int Quantity { get; set; }
        public int OrderQuantity { get; set; }
        public int OrderPoint { get; set; }

        public int CategoryId { get; set; }
        public int ShelfId { get; set; }
    }
}
