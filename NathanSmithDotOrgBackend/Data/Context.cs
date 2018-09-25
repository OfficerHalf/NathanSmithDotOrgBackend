using Microsoft.EntityFrameworkCore;

namespace NathanSmithDotOrgBackend.Data
{
    public class DataContext : DbContext
    {
        public DbSet<AccountEntity> Accounts { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // TODO: Don't hardcode the DB name. This is for testing only.
            string db = "accounts.db";
            optionsBuilder.UseSqlite($"Data Source={db}");
        }
    }
}
