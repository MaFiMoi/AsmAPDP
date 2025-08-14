using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SIMSWebApp.Models
{
    public class Enrollments : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public class Enrollment
        {
            [Key]
            public int EnrollmentID { get; set; }

            [Required]
            public int StudentID { get; set; }
            [ForeignKey("StudentID")]
            public Student Student { get; set; }

            [Required]
            public int CourseID { get; set; }
            [ForeignKey("CourseID")]
            public Course Course { get; set; }

            public DateTime EnrollmentDate { get; set; } = DateTime.Now;
        }
    }
}
