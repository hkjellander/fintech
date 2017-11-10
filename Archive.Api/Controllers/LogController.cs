using Archive.Api.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;

namespace Archive.Api.Controllers
{
    [Route("[controller]")]
    public class LogController : Controller
    {
        private readonly LogContext _context;
        private readonly ILogger _logger;

        public LogController(LogContext context, ILogger logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpPost("/add")]
        public IActionResult Add([FromBody] LogEntry logEntry)
        {
            _context.Logs.Add(logEntry);
            var count = _context.SaveChanges();
            _logger.LogDebug("{0} records saved to database", count);
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
            _logger.LogDebug("Returning log entry: " + entry.Json);
            return new ObjectResult(entry);
        }

        [HttpGet("/log")]
        public IEnumerable<LogEntry> GetAll()
        {
            IEnumerable<LogEntry> logs = _context.Logs.ToList();
            if (logs.Count() > 0)
            {
                _logger.LogDebug("Got logs from database:");
                foreach (LogEntry log in logs)
                {
                    _logger.LogDebug("  ID: " + log.Id + " : " + log.Json);
                }
            }
            return logs;
        }
    }
}
