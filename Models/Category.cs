using MoneyPilot.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace MoneyPilot.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;
        public string? Icon { get; set; }

        public CategoryNature CategoryNature { get; set; } = CategoryNature.NONE;

        [JsonIgnore]
        public bool IsVisible { get; set; } = true;

        [JsonIgnore]
        public int? ParentCategoryId { get; set; }
        public Category? ParentCategory { get; set; }
        public ICollection<Category> SubCategories { get; set; }

        [InverseProperty("Category")]
        [JsonIgnore]
        public ICollection<Transaction>? Transactions { get; set; } = [];


        public Category(string name, string? icon, CategoryNature categoryNature, bool isVisible)
        {
            Name = name;
            Icon = icon;
            CategoryNature = categoryNature;
            IsVisible = isVisible;
        }
    }
}
