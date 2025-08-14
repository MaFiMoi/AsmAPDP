using System;
using System.ComponentModel.DataAnnotations;

namespace SIMSWebApp.Models
{
    public class Course
    {
        [Key]
        public int CourseID { get; set; }

        [Required, MaxLength(50)]
        public string CourseCode { get; set; }

        [Required, MaxLength(100)]
        public string CourseName { get; set; }

        public string Description { get; set; }
        public int Credits { get; set; }
        public string Department { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
