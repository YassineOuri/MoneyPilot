using Microsoft.EntityFrameworkCore;

namespace MoneyPilot.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        {

        }
    }
    
}
