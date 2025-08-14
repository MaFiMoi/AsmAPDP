using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SIMSWebApp.DatabaseContext;
using SIMSWebApp.Models;
using System.Linq;

namespace SIMSWebApp.Controllers
{
    [Authorize] // Bắt buộc đăng nhập
    public class StudentCourseListController : Controller
    {
        private readonly SIMSDbContext _context;

        public StudentCourseListController(SIMSDbContext context)
        {
            _context = context;
        }

        // Xem danh sách
        [HttpGet]
        [Authorize(Roles = "Admin,Faculty,Student")]
        public IActionResult Index()
        {
            var enrollments = _context.Set<Enrollments.Enrollment>()
                .Include(e => e.Student)
                .Include(e => e.Course)
                .ToList();

            // Nếu là Student thì chỉ hiển thị các course của chính họ
            if (User.IsInRole("Student"))
            {
                var username = User.Identity.Name;

                // Tìm UserID của người đang đăng nhập
                var user = _context.Users.FirstOrDefault(u => u.Username == username);

                if (user != null)
                {
                    var student = _context.Students.FirstOrDefault(s => s.UserID == user.UserID);

                    if (student != null)
                    {
                        enrollments = enrollments
                            .Where(e => e.StudentID == student.StudentID)
                            .ToList();
                    }
                    else
                    {
                        enrollments = new List<Enrollments.Enrollment>();
                    }
                }
                else
                {
                    enrollments = new List<Enrollments.Enrollment>();
                }
            }

            return View(enrollments);
        }

        // Xóa enrollment
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult Delete(int id)
        {
            var enrollment = _context.Set<Enrollments.Enrollment>()
                .FirstOrDefault(e => e.EnrollmentID == id);

            if (enrollment == null)
            {
                TempData["ErrorMessage"] = "Enrollment not found.";
                return RedirectToAction("Index");
            }

            _context.Remove(enrollment);
            _context.SaveChanges();

            TempData["SuccessMessage"] = "Student removed from course successfully.";
            return RedirectToAction("Index");
        }
    }
}
