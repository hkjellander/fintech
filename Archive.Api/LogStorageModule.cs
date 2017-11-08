using Nancy;
using Nancy.ModelBinding;
using System;
using DbLogStorage;
using System.Collections.Generic;

namespace Archive.Api
{
    public class LogStorageModule : NancyModule
    {
        public LogStorageModule() : base("/")
        {
            Get("/logs", _ =>
            {
                using (var context = new LogContext())
                {
                    LogStorage logStorage = new LogStorage(context);
                    IEnumerable<LogEntry> logs = logStorage.GetAll();
                    Console.WriteLine("Got logs from database:");
                    foreach (LogEntry log in logs)
                    {
                        Console.WriteLine(log.ToString());
                    }
                    return Response.AsJson(logs);
                }
            });
            Post("/add", _ =>
            {
                var newEntry = this.Bind<LogEntry>();
                using (var context = new LogContext())
                {
                    LogStorage logStorage = new LogStorage(context);
                    logStorage.Add(newEntry);
                    Console.WriteLine("Wrote log entry to database: " +
                                      newEntry.ToString());
                }
                return this.Negotiate
                           .WithStatusCode(HttpStatusCode.Created);
            });
        }
    }
}
