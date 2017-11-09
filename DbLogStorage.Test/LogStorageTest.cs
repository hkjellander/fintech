using DbLogStorage;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using Xunit;

namespace DbLogStorage.Test
{
    public class LogStorageTest
    {
        [Fact]
        public void SaveLogWithSimpleDataWorks()
        {
            using (var connection = new SqliteConnection("DataSource=:memory:"))
            {
                connection.Open();
                DbContextOptions<LogContext> options = SetupDatabase(connection);

                // Run the test against one instance of the context
                using (var context = new LogContext(options))
                {
                    LogStorage logStorage = new LogStorage(context);
                    logStorage.Add(GetTestData());
                }

                // Use a separate instance of the context to verify correct data was saved to database
                using (var context = new LogContext(options))
                {
                    LogStorage logStorage = new LogStorage(context);
                    logStorage.GetAll();
                }
            }
        }

        private DbContextOptions<LogContext> SetupDatabase(
            SqliteConnection connection)
        {
            var options = new DbContextOptionsBuilder<LogContext>()
                    .UseSqlite(connection)
                    .Options;
            return options;
        }

        private LogEntry GetTestData()
        {
            IDictionary<string, string> data = new Dictionary<string, string>();
            data["foo"] = "bar";
            return new LogEntry()
            {
                Id = 0,
                Json = JsonConvert.SerializeObject(data),
            };
        }
    }
}
