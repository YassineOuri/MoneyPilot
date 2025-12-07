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

        public bool IsVisible { get; set; } = true;

        public int ParentCategoryId { get; set; }

        [ForeignKey("ParentCategoryId")]
        [JsonIgnore]
        public Category? ParentCategory { get; set; }

        [InverseProperty("ParentCategory")]
        [JsonIgnore]
        public ICollection<Category>? SubCategories { get; set; } = [];

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
