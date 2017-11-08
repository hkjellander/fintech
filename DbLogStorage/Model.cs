using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DbLogStorage
{
    public class LogContext : DbContext
    {
        public LogContext()
        { }

        // For testing purposes.
        public LogContext(DbContextOptions<LogContext> options)
            : base(options)
        { }

        public DbSet<LogEntry> Logs { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlite("Data Source=logs.sqlite");
            }
        }
    }

    public class LogEntry
    {
        public int Id { get; set; }
        [Required]
        public string Json { get; set; }
    }
}
