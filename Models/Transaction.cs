using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MoneyPilot.Models
{
    public class Transaction
    {

        [Key]
        public int Id { get; set; }
        public double Amount { get; set; } = 0;

        public int AccountId { get; set; }

        [ForeignKey("AccountId")]
        public Account? Account { get; set; }

        public int OwnerId { get; set; }

        [ForeignKey("OwnerId")]
        public User? Owner { get; set; }

        public DateTime DateTime { get; set; } = DateTime.Now;

        public TransactionType TransactionType { get; set; } = TransactionType.EXPENSE;

        public string Note { get; set; } = string.Empty;

        public Transaction(double amount, int accountId, int ownerId, DateTime dateTime, TransactionType transactionType, string note)
        {
            Amount = amount;
            AccountId = accountId;
            OwnerId = ownerId;
            DateTime = dateTime;
            TransactionType = transactionType;
            Note = note;
        }
    }



}
