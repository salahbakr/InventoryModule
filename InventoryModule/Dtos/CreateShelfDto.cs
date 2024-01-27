using System.ComponentModel.DataAnnotations;

namespace InventoryModule.Dtos
{
    public class CreateShelfDto
    {
        [Required]
        [MaxLength(10)]
        public string ReferenceNumber { get; set; }
    }
}
