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
    }
}
