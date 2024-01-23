namespace InventoryModule.Entities
{
    public class Shelf
    {
        public int Id { get; set; }
        public string ReferenceNumber { get; set; }
        public ICollection<Item> Items { get; set; }
    }
}
