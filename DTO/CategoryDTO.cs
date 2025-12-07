using MoneyPilot.Enums;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;

namespace MoneyPilot.DTO
{
    public class CategoryDTO
    {
        [SwaggerIgnore]
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [MaxLength(100, ErrorMessage = "Name cannot exceed 100 characters")]
        public required string Name { get; set; }

        [MaxLength(50, ErrorMessage = "Icon cannot exceed 50 characters")]
        public string? Icon { get; set; }

        [Required(ErrorMessage = "Category nature is required")]
        [EnumDataType(typeof(CategoryNature), ErrorMessage = "Category nature is invalid")]
        public CategoryNature CategoryNature { get; set; } = CategoryNature.NONE;

        public bool IsVisible { get; set; } = true;

        [Required(ErrorMessage = "Parent category ID is required")]
        public required int ParentCategoryId { get; set; }
    }
}

