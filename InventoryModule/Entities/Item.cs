namespace InventoryModule.Entities
{
    public class Item
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Quantity { get; set; }
        public DateTime DateEntered { get; set; } = DateTime.UtcNow;

        public Category Category { get; set; }
        public Shelf Shelf { get; set; }
    }
}
