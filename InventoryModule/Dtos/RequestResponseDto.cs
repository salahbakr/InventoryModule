using InventoryModule.Entities;
using InventoryModule.Enums;

namespace InventoryModule.Dtos
{
    public class RequestResponseDto
    {
        public int Id { get; set; }
        public string From { get; set; }
        public DateTime DateExpected { get; set; }
        public Status Status { get; set; }
        public DateTime RequestDate { get; set; }

        public ICollection<RequestItemResponseDto> RequestItems { get; set; }
    }
}
