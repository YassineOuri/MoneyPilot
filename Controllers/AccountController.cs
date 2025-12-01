using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MoneyPilot.Data;
using MoneyPilot.DTO;
using MoneyPilot.Models;
using System.Security.Principal;

namespace MoneyPilot.Controllers
{
    [Route("api/accounts")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public AccountController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<Account>>> GetAccounts()
        {
            List<Account> accounts = await _context.Accounts.ToListAsync();
            return Ok(accounts);
        }


        [HttpPost]
        public async Task<IActionResult> addAccount([FromBody] AccountDTO account)
        {   

            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

  
            _context.Add(account);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(addAccount), new { id = account.Id }, account);
        }




        [HttpPut("{id:int}")]
        public async Task<IActionResult> updateAccount([FromBody] Account accountToUpdate, int id)
        {   
           
            var exsistingAccount = await _context.FindAsync<Account>(id);
            if (exsistingAccount == null) return NotFound("Requested account not found");

            exsistingAccount.Name = accountToUpdate.Name;
            exsistingAccount.Color = accountToUpdate.Color;
            exsistingAccount.InitialAmount = accountToUpdate.InitialAmount;
            exsistingAccount.Type = accountToUpdate.Type;
            exsistingAccount.currency = accountToUpdate.currency;
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(updateAccount), new { id = exsistingAccount.Id }, exsistingAccount);
        }


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