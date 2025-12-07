using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace MoneyPilot.Models
{
    public class Account
    {
        [Key]
        
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Color { get; set; }
        public AccountType Type { get; set; } = AccountType.Cash;
        public double InitialAmount { get; set; } = 0;

      
        public AccountCurrency Currency { get; set; } = AccountCurrency.USD;

        [Required]
        public int OwnerId { get; set; }

        [ForeignKey("OwnerId")]
        [JsonIgnore]
        public User? Owner { get; set; }

        [InverseProperty("Account")]
        [JsonIgnore]
        public ICollection<Transaction>? Transactions { get; set; } = [];


        public Account()
        {

        }

        public Account(string name, string color, AccountType type, double initialAmount, AccountCurrency currency, int ownerId)
        {
            Name = name;
            Color = color;
            Type = type;
            InitialAmount = initialAmount;
            Currency = currency;
            OwnerId = ownerId;
        }
    }
}
