using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MoneyPilot.Data;
using MoneyPilot.DTO;
using MoneyPilot.Models;
using MoneyPilot.Services;
using System.Security.Claims;

namespace MoneyPilot.Controllers
{
    [ApiController]
    [Route("api/transactions")]
    public class TransactionController(
        ApplicationDbContext context, 
        AuthService authService) : Controller
    {

        private readonly ApplicationDbContext _context = context;
        private readonly AuthService _authService = authService;

        [Authorize]
        [HttpGet("UserId")]
        
        public async Task<ActionResult<List<Transaction>>> GetTransactions(int UserId)
        {
            var transactions = await _context.Transactions.Where(t => t.OwnerId == UserId).ToListAsync<Transaction>();

            return Ok(transactions);
        }

        [Authorize]
        [HttpPost]
        
        public async Task<ActionResult> AddTransactions([FromBody] StoreTransactionDTO transaction)
        {   

            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            ClaimsPrincipal currentUser = User;
            int userId = await _authService.getCurrentUserID(currentUser);

            var newTransaction = new Transaction(
                transaction.Amount,
                transaction.AccountId,
                userId,
                transaction.DateTime ?? DateTime.Now,
                transaction.TransactionType,
                transaction.Note
                );

            _context.Transactions.Add(newTransaction);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(AddTransactions), new { Id = newTransaction.Id }, newTransaction);
        }
    }
}
