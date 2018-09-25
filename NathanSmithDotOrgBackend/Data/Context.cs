using Microsoft.EntityFrameworkCore;


namespace NathanSmithDotOrgBackend.Data
{
    public class DataContext : DbContext
    {
        public DbSet<AccountEntity> Accounts { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string db = "accounts.db";
            optionsBuilder.UseSqlite($"Data Source={db}");
        }
    }
}
