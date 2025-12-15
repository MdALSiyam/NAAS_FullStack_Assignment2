using System.ComponentModel.DataAnnotations;

namespace ProductManagement.Application.DTOs
{
    public class CategoryDto
    {
        public string Id { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 1)]
        public string Name { get; set; }
    }
}