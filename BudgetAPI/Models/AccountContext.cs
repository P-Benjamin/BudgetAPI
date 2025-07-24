using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace BudgetAPI.Models
{
    public class AccountContext : DbContext
    {
            public AccountContext(DbContextOptions<AccountContext> options) : base(options)
            {
            }

            public DbSet<Income> Income { get; set; }
            public DbSet<Outcome> Outcome { get; set; }
            public DbSet<User> User { get; set; }

            public DbSet<Source> Source { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Income>()
                .HasOne(i => i.Source)
                .WithMany() 
                .HasForeignKey(i => i.SourceId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Income>()
                .HasOne(i => i.Source)
                .WithMany()
                .HasForeignKey(i => i.SourceId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
