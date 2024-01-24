using System.ComponentModel.DataAnnotations;

namespace InventoryModule.Dtos
{
    public class CategoryResponseDto
    {
        public int Id { get; set; }

        [MaxLength(50)]
        public string Name { get; set; }
    }
}
