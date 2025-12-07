using Microsoft.EntityFrameworkCore;
using MoneyPilot.Models;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;
using System.Net.NetworkInformation;

namespace MoneyPilot.DTO
{
    public class AccountDTO
    {


        [SwaggerIgnore]
        public int Id { get; set; }

        [Required]
        public required string Name { get; set; }


        [Required]
        [RegularExpression(@"^#([0-9a-fA-F]{6}|[0-9a-fA-F]{3})$", ErrorMessage = "Color must be a hex value")]
        public required string Color { get; set; }

        [Required]
        [EnumDataType(typeof(AccountType), ErrorMessage = "Account Type is invalid")]
        public required AccountType Type { get; set; } = AccountType.Cash;

        [Required]
        [Range(0, 10000000)]
        public double InitialAmount { get; set; } = 0;

        [Required]
        [EnumDataType(typeof(AccountCurrency), ErrorMessage = "Account currency is invalid")]
        public AccountCurrency Currency { get; set; } = AccountCurrency.USD;
    }
}
