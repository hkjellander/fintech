using System;
using System.Collections.Generic;
using System.Linq;

namespace DbLogStorage
{
    public class LogStorage
    {
        private LogContext context;

        public LogStorage(LogContext context)
        {
            this.context = context;
            context.Database.EnsureCreated();
        }

        public void Add(LogEntry logEntry)
        {
            context.Logs.Add(logEntry);
            var count = context.SaveChanges();
            Console.WriteLine("{0} records saved to database", count);
        }

        public IEnumerable<LogEntry> GetAll()
        {
            return context.Logs.ToList();
        }
    }
}