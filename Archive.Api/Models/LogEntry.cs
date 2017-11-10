using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Archive.Api.Models
{
    public class LogEntry
    {
        public long Id { get; set; }
        [Required]
        public string Json { get; set; }
    }
}
