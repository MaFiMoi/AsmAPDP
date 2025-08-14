using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SIMSWebApp.DatabaseContext;
using SIMSWebApp.DatabaseContext.Entities;
using System;
using System.Linq;
using static SIMSWebApp.Models.Enrollments;


namespace SIMSWebApp.Controllers
{
    [Authorize]
    public class AddStudentToCourseController : Controller
    {
        private readonly SIMSDbContext _context;

        public AddStudentToCourseController(SIMSDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult Index()
        {
            ViewBag.Students = _context.Students.ToList();
            ViewBag.Courses = _context.Courses.ToList();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public IActionResult Index(int studentId, int courseId)
        {
            // Kiểm tra xem đã tồn tại chưa
            var exists = _context.Enrollments
                .Any(e => e.StudentID == studentId && e.CourseID == courseId);

            if (exists)
            {
                ModelState.AddModelError("", "Sinh viên này đã được đăng ký vào khóa học này.");
            }
            else
            {
                var enrollment = new Enrollment
                {
                    StudentID = studentId,
                    CourseID = courseId,
                    EnrollmentDate = DateTime.Now
                };
                _context.Enrollments.Add(enrollment);
                _context.SaveChanges();
                TempData["SuccessMessage"] = "Thêm sinh viên vào khóa học thành công!";
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Students = _context.Students.ToList();
            ViewBag.Courses = _context.Courses.ToList();
            return View();
        }
    }
}
