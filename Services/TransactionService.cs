using MoneyPilot.Data;
using MoneyPilot.Enums;
using MoneyPilot.Models;
using System.Threading.Tasks;

namespace MoneyPilot.Services
{
    public class TransactionService(ApplicationDbContext context)
    {
        private readonly ApplicationDbContext _context = context;

        public async Task ApplyTransaction(Transaction transaction)
        {   
           
            var account = await _context.Accounts.FindAsync(transaction.AccountId);

            if (account == null)
            {
                throw new KeyNotFoundException("Transaction account not found");
            }

            switch(transaction.TransactionType)
            {
                case TransactionType.EXPENSE:
                    {
                        account.InitialAmount -= transaction.Amount;
                        await _context.SaveChangesAsync();
                       

                    } break;
                case TransactionType.INCOME:
                    {
                        account.InitialAmount += transaction.Amount;
                        await _context.SaveChangesAsync();

                    } break;
                default: throw new Exception("Transaction type is not valid");
            }
        }




        public async Task DenyTransaction(Transaction transaction)
        {

            var account = await _context.Accounts.FindAsync(transaction.AccountId);

            if (account == null)
            {
                throw new KeyNotFoundException("Transaction account not found");
            }

            switch (transaction.TransactionType)
            {
                case TransactionType.EXPENSE:
                    {
                        account.InitialAmount += transaction.Amount;
                        await _context.SaveChangesAsync();


                    }
                    break;
                case TransactionType.INCOME:
                    {
                        account.InitialAmount -= transaction.Amount;
                        await _context.SaveChangesAsync();

                    }
                    break;
                default: throw new Exception("Transaction type is not valid");
            }
        }
    }
}
