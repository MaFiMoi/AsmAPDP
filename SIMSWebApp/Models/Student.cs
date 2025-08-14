using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using SIMSWebApp.DatabaseContext.Entities; // để dùng Class

namespace SIMSWebApp.Models
{
    [Table("Students")]
    public class Student
    {
        [Key]
        public int StudentID { get; set; }

        public int? UserID { get; set; }

        [Required]
        [StringLength(50)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(50)]
        public string LastName { get; set; }

        [DataType(DataType.Date)]
        public DateTime? DateOfBirth { get; set; }

        [StringLength(10)]
        public string Gender { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        [StringLength(15)]
        public string Phone { get; set; }

        public string Address { get; set; }

        [DataType(DataType.Date)]
        public DateTime? EnrollmentDate { get; set; }

        [StringLength(50)]
        public string Program { get; set; }

        // FK tới Class
        public int? ClassId { get; set; }

        [ForeignKey("ClassId")]
        public Class? Class { get; set; }
    }
}
