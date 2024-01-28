using InventoryModule.Enums;

namespace InventoryModule.Dtos
{
    public class CreateRequestDto
    {
        public string From { get; set; }
        public DateTime DateExpected { get; set; }
        public Status Status { get; set; }

        public ICollection<CreateRequestItemsDto> RequestItems { get; set; }
    }
}
