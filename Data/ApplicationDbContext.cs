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

        public DbSet<User> Users { get; set; }

        public DbSet<Transaction> Transactions { get; set; }

        public DbSet<Category> Categories { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasMany(e => e.Accounts)
                .WithOne(e => e.Owner)
                .HasForeignKey(e => e.OwnerId)
                .IsRequired(true)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<User>()
                .HasMany(e => e.Transactions)
                .WithOne(e => e.Owner)
                .HasForeignKey(e => e.OwnerId)
                .IsRequired(true)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Account>()
                .HasMany(e => e.Transactions)
                .WithOne(e => e.Account)
                .HasForeignKey(e => e.AccountId)
                .IsRequired(true)
                .OnDelete(DeleteBehavior.Restrict);


            modelBuilder.Entity<Category>()
            .HasOne(c => c.ParentCategory)
            .WithMany(c => c.SubCategories)
            .HasForeignKey(c => c.ParentCategoryId)
            .OnDelete(DeleteBehavior.Restrict);



        }
    }
    
}
