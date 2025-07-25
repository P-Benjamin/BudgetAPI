using Microsoft.EntityFrameworkCore;

namespace BudgetAPI.Models
{
    /// <summary>
    /// Contexte de base de données pour l'application BudgetAPI.
    /// Gère les entités Income, Outcome, Source et User.
    /// </summary>
    public class AccountContext : DbContext
    {
        public AccountContext(DbContextOptions<AccountContext> options) : base(options) { }

        /// <summary>
        /// Revenus enregistrés.
        /// </summary>
        public DbSet<Income> Income { get; set; }

        /// <summary>
        /// Dépenses enregistrées.
        /// </summary>
        public DbSet<Outcome> Outcome { get; set; }

        /// <summary>
        /// Utilisateurs du système.
        /// </summary>
        public DbSet<User> User { get; set; }

        /// <summary>
        /// Sources de revenus/dépenses (ex: Salaire, Loyer).
        /// </summary>
        public DbSet<Source> Source { get; set; }

        /// <summary>
        /// Configuration des relations entre entités.
        /// </summary>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Income>()
                .HasOne(i => i.Source)
                .WithMany()
                .HasForeignKey(i => i.SourceId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Outcome>()
                .HasOne(o => o.Source)
                .WithMany()
                .HasForeignKey(o => o.SourceId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
