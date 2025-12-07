using System.ComponentModel.DataAnnotations;
using Swashbuckle.AspNetCore.Annotations;
using MoneyPilot.Enums;

namespace MoneyPilot.DTO.Transactions
{
    public class StoreTransactionDTO
    {
        [SwaggerIgnore]
        public int Id { get; set; }

        [Required(ErrorMessage = "Amount is required")]
        [Range(0.01, 10000000, ErrorMessage = "Amount must be between 0.01 and 10,000,000")]
        public double Amount { get; set; }

        [Required(ErrorMessage = "Account ID is required")]
        public required int AccountId { get; set; }


        [Required(ErrorMessage = "Category ID is required")]
        public required int CategoryId { get; set; }

        [Required(ErrorMessage = "Transaction type is required")]
        [EnumDataType(typeof(TransactionType), ErrorMessage = "Transaction type must be either EXPENSE or INCOME")]
        public TransactionType TransactionType { get; set; } = TransactionType.EXPENSE;

        [MaxLength(500, ErrorMessage = "Note cannot exceed 500 characters")]
        public string Note { get; set; } = string.Empty;

       
        public DateTime? DateTime { get; set; }
    }
}

