using Archive.Api.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Archive.Api.Controllers
{
    [Route("[controller]")]
    public class LogController : Controller
    {
        private readonly LogContext _context;

        public LogController(LogContext context)
        {
            _context = context;
        }

        [HttpPost("/add")]
        public IActionResult Add([FromBody] LogEntry logEntry)
        {
            _context.Logs.Add(logEntry);
            var count = _context.SaveChanges();
            Console.WriteLine("{0} records saved to database", count);
            return CreatedAtRoute("GetLog", new { id = logEntry.Id }, logEntry);
        }

        [HttpGet("/log/{id}", Name = "GetLog")]
        public IActionResult GetById(long id)
        {
            var entry = _context.Logs.FirstOrDefault(t => t.Id == id);
            if (entry == null)
            {
                return NotFound();
            }
            Console.WriteLine("Returning single log entry: " + entry.Json);
            return new ObjectResult(entry);
        }

        [HttpGet("/log")]
        public IEnumerable<LogEntry> GetAll()
        {
            IEnumerable<LogEntry> logs = _context.Logs.ToList();
            Console.WriteLine("Got logs from database:");
            foreach (LogEntry log in logs)
            {
                Console.WriteLine("  ID: " + log.Id + " : " + log.Json);
            }
            return logs;
        }
    }
}
