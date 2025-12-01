using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace MoneyPilot.Models
{
    public class Account
    {
        [Key]
        [SwaggerIgnore]
        public int Id { get; set; }
        [Required]
        public required string Name { get; set; }
        [Required]
        public required string Color { get; set; }
        public required AccountType Type { get; set; } = AccountType.Cash;
        public double InitialAmount { get; set; } = 0;

      
        public AccountCurrency currency { get; set; } = AccountCurrency.USD;




    }
}
