using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace DbLogStorage
{
    public class LogContext : DbContext
    {
        public DbSet<LogEntry> Logs { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=logs.db");
        }
    }

    public class LogEntry
    {
        public int Id { get; set; }
        public string Json { get; set; }
    }
}
