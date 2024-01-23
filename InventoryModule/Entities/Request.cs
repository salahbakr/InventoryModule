using InventoryModule.Enums;

namespace InventoryModule.Entities
{
    public class Request
    {
        public int Id { get; set; }
        public string From { get; set; }
        public DateTime DateExpected { get; set; }
        public Status Status { get; set; }
        public DateTime RequestDate { get; set; } = DateTime.Now;

        public ICollection<RequestItem> RequestItems { get; set; }
    }
}