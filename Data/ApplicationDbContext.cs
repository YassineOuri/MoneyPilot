using Microsoft.EntityFrameworkCore;
using MoneyPilot.Models;

namespace MoneyPilot.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }


        public DbSet<Account> Accounts { get; set; }
    }
    
}
