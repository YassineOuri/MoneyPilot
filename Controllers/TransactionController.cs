using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MoneyPilot.Data;
using MoneyPilot.DTO.Transactions;
using MoneyPilot.Models;
using MoneyPilot.Services;
using System.Security.Claims;

namespace MoneyPilot.Controllers
{
    [ApiController]
    [Route("api/transactions")]
    public class TransactionController(
        ApplicationDbContext context,
        AuthService authService,
        TransactionService transactionService
        ) : Controller
    {

        private readonly ApplicationDbContext _context = context;
        private readonly AuthService _authService = authService;
        private readonly TransactionService _transactionService = transactionService;

        [Authorize]
        [HttpGet("user/{UserId}")]

        public async Task<ActionResult<List<Transaction>>> GetTransactions(int UserId)
        {
         

            if(!await _authService.ValidateUserOwnership(User, UserId))
            {
                return Unauthorized("You are not authorized to perform this action");
            }

            var transactions = await _context.Transactions.Where(t => t.OwnerId == UserId).ToListAsync<Transaction>();

            return Ok(transactions);
        }


        [Authorize]
        [HttpGet("account/{AccountId}")]
        public async Task<ActionResult<List<Transaction>>> GetTransactionsByAccount(int AccountId)
        {
            var account = await _context.Accounts.FindAsync(AccountId);
            if (account == null)
            {
                return NotFound("Account not found");
            }

            if (!await _authService.ValidateUserOwnership(User, account.OwnerId))
            {
                return Unauthorized("You are not authorized to perform this action");
            }

            var transactions = await _context.Transactions.Where(t => t.AccountId == AccountId).ToListAsync();
            return Ok(transactions);
        }

        [Authorize]
        [HttpPost]

        public async Task<ActionResult> AddTransactions([FromBody] StoreTransactionDTO transaction)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                ClaimsPrincipal currentUser = User;
                int userId = await _authService.getCurrentUserID(currentUser);

                var account = await _context.Accounts.FindAsync(transaction.AccountId);
                if (account == null)
                {
                    return NotFound("Account not found");
                }

                if (!await _authService.ValidateUserOwnership(User, account.OwnerId))
                {
                    return Unauthorized("You are not authorized to add transactions to this account");
                }

                var newTransaction = new Transaction(
                    transaction.Amount,
                    transaction.AccountId,
                    userId,
                    transaction.DateTime ?? DateTime.Now,
                    transaction.TransactionType,
                    transaction.Note
                    );

                using var databaseTransaction = _context.Database.BeginTransaction();

                _context.Transactions.Add(newTransaction);
                await _context.SaveChangesAsync();
                await _transactionService.ApplyTransaction(newTransaction);
                await databaseTransaction.CommitAsync();
                return CreatedAtAction(nameof(AddTransactions), new { Id = newTransaction.Id }, newTransaction);
            }
            catch (Exception e)
            {
                if (_context.Database.CurrentTransaction != null)
                {
                    await _context.Database.CurrentTransaction.RollbackAsync();
                }

                return StatusCode(500, new
                {
                    message = "Something went wrong",
                    error = e.Message
                });
            }
        }


        [Authorize]
        [HttpPut("{id:int}")]
        public async Task<ActionResult<Transaction>> UpdateTransaction([FromBody] UpdateTransactionDTO transactionToUpdate, int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existingTransaction = await _context.Transactions.FindAsync(id);

            if (existingTransaction == null)
            {
                return NotFound("Requested transaction not found");
            }

            if (!await _authService.ValidateUserOwnership(User, existingTransaction.OwnerId))
            {
                return Unauthorized("You are not authorized to perform actions on this transaction");
            }


            try
            {
                using var databaseTransaction = _context.Database.BeginTransaction();

                await _transactionService.DenyTransaction(existingTransaction);

                existingTransaction.Amount = transactionToUpdate.Amount;
                existingTransaction.AccountId = transactionToUpdate.AccountId;
                existingTransaction.TransactionType = transactionToUpdate.TransactionType;
                existingTransaction.Note = transactionToUpdate.Note;
                existingTransaction.DateTime = transactionToUpdate.DateTime ?? existingTransaction.DateTime;

                await _transactionService.ApplyTransaction(existingTransaction);

                await _context.SaveChangesAsync();
                await databaseTransaction.CommitAsync();

                return CreatedAtAction(nameof(UpdateTransaction), new { existingTransaction.Id }, existingTransaction);



            } catch (Exception e)
            {
                if (_context.Database.CurrentTransaction != null)
                {
                    await _context.Database.CurrentTransaction.RollbackAsync();
                }

                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    message = "Something went wrong",
                    error = e.Message
                });
            }

            

        }


        [HttpDelete("{id:int}")]
        public async Task<ActionResult> DeleteTransaction(int id)
        {
            try
            {
                var existingTransaction = await _context.Transactions.FindAsync(id);

                if (existingTransaction == null)
                {
                    return NotFound("Requested transaction not found");
                }

                if (!await _authService.ValidateUserOwnership(User, existingTransaction.OwnerId))
                {
                    return Unauthorized("You are not authorized to perform actions on this transaction");
                }

                using var databaseTransaction = await _context.Database.BeginTransactionAsync();
                await _transactionService.DenyTransaction(existingTransaction);
                _context.Transactions.Remove(existingTransaction);
                await _context.SaveChangesAsync();
                await databaseTransaction.CommitAsync();

                return Ok("Transaction deleted successfully");
            }
            catch (Exception e)
            {
                if (_context.Database.CurrentTransaction != null)
                {
                    await _context.Database.CurrentTransaction.RollbackAsync();
                }

                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    message = "Something went wrong",
                    error = e.Message
                });
            }
        }
    }
}
