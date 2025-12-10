using MoneyPilot.Enums;
using MoneyPilot.Models;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

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

        public int? ParentCategoryId { get; set; }


        [JsonIgnore]
        public ICollection<Category>? SubCategories { get; set; }
    }
}

