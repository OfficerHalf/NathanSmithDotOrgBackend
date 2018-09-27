using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using NathanSmithDotOrgBackend.Configuration;
using Microsoft.Extensions.Options;

namespace NathanSmithDotOrgBackend.Data
{
    public class DataContext : DbContext
    {
        public DbSet<AccountEntity> Accounts { get; set; }
        private SqliteConfigurationOptions _sqlConfig;
        public DataContext(IOptions<SqliteConfigurationOptions> sqlConfig)
        {
            _sqlConfig = sqlConfig.Value;
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite($"Data Source={_sqlConfig.Name}");
        }
    }
}
