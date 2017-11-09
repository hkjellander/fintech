using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Archive.Api.Models
{
    public class LogContext : DbContext
    {
        public LogContext()
        { }

        // For testing purposes.
        public LogContext(DbContextOptions<LogContext> options)
            : base(options)
        { }

        public virtual DbSet<LogEntry> Logs { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlite("Data Source=logs.sqlite");
            }
        }
    }
}
