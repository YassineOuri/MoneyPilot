using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MoneyPilot.Data;
using MoneyPilot.DTO;
using MoneyPilot.Models;
using MoneyPilot.Services;
using System.Security.Claims;
using System.Security.Principal;

namespace MoneyPilot.Controllers
{
    [Route("api/accounts")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly AuthService _authService;

        
        public AccountController(ApplicationDbContext context, AuthService authService)
        {
            _context = context;
            _authService = authService;
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<List<Account>>> GetAccounts()
        {
            List<Account> accounts = await _context.Accounts.ToListAsync();
            return Ok(accounts);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> AddAccount([FromBody] AccountDTO account) 
        {   
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            ClaimsPrincipal currentUser = User;
            int userId = await _authService.getCurrentUserID(currentUser);

            var newAccount = new Account
            (
                account.Name,
                account.Color,
                account.Type,
                account.InitialAmount,
                account.Currency,
                userId
            );

            _context.Accounts.Add(newAccount);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(AddAccount), new { id = newAccount.Id }, newAccount);
        }


        [Authorize]
        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateAccount([FromBody] AccountDTO accountToUpdate, int id)
        {   
           
            var exsistingAccount = await _context.FindAsync<Account>(id);
            if (exsistingAccount == null) return NotFound("Requested account not found");


            exsistingAccount.Name = accountToUpdate.Name;
            exsistingAccount.Color = accountToUpdate.Color;
            exsistingAccount.InitialAmount = accountToUpdate.InitialAmount;
            exsistingAccount.Type = accountToUpdate.Type;
            exsistingAccount.Currency = accountToUpdate.Currency;
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(UpdateAccount), new { id = exsistingAccount.Id }, exsistingAccount);
        }

        [Authorize]
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> deleteAccount(int id)
        {
            var account = await _context.FindAsync<Account>(id);
            if(account == null)
            {
                return NotFound("Requested account not found");
            }

            _context.Remove<Account>(account);
            await _context.SaveChangesAsync();

            return Ok($"Account of Id {account.Id} deleted successfully");
        }
    }
}