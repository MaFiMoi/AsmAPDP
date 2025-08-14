using SIMSWebApp.Models;
using System;
using System.Collections.Generic;

namespace SIMSWebApp.DatabaseContext.Entities
{
    public class Class
    {
        public int ClassId { get; set; }
        public string ClassName { get; set; } = null!;
        public string? Description { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        // Navigation property: 1 Class -> nhiều Student
        public ICollection<Student> Students { get; set; } = new List<Student>();
    }
}
