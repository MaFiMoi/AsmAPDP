using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SIMSWebApp.DatabaseContext;
using SIMSWebApp.Models;
using System.Linq;

namespace SIMSWebApp.Controllers
{
    [Authorize]
    public class CourseController : Controller
    {
        private readonly SIMSDbContext _context;

        public CourseController(SIMSDbContext context)
        {
            _context = context;
        }

        // Hiển thị danh sách khóa học
        [HttpGet]
        [Authorize(Roles = "Admin,Student,Faculty")]
        public IActionResult Index()
        {
            var courses = _context.Courses.ToList();
            return View(courses);
        }

        // Hiển thị form thêm khóa học
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }

        // Xử lý thêm khóa học
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public IActionResult Create(Course course)
        {
            if (ModelState.IsValid)
            {
                course.CreatedAt = System.DateTime.Now; // Tự gán ngày tạo
                _context.Courses.Add(course);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(course);
        }

        // Hiển thị form chỉnh sửa khóa học
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult Edit(int id)
        {
            var course = _context.Courses.FirstOrDefault(c => c.CourseID == id);
            if (course == null)
                return NotFound();

            return View(course);
        }

        // Xử lý lưu chỉnh sửa khóa học
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public IActionResult Edit(Course course)
        {
            if (ModelState.IsValid)
            {
                var existingCourse = _context.Courses.FirstOrDefault(c => c.CourseID == course.CourseID);
                if (existingCourse == null)
                    return NotFound();

                existingCourse.CourseCode = course.CourseCode;
                existingCourse.CourseName = course.CourseName;
                existingCourse.Description = course.Description;
                existingCourse.Credits = course.Credits;
                existingCourse.Department = course.Department;
                // Thường CreatedAt không thay đổi khi chỉnh sửa

                _context.SaveChanges();

                return RedirectToAction(nameof(Index));
            }
            return View(course);
        }

        // Hiển thị xác nhận xóa khóa học
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult Delete(int id)
        {
            var course = _context.Courses.FirstOrDefault(c => c.CourseID == id);
            if (course == null)
                return NotFound();

            return View(course);
        }

        // Xử lý xóa khóa học
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public IActionResult DeleteConfirmed(int id)
        {
            var course = _context.Courses
                .FirstOrDefault(c => c.CourseID == id);

            if (course == null)
                return NotFound();

            // Kiểm tra nếu có sinh viên đăng ký khóa học này
            bool hasStudents = _context.Enrollments.Any(e => e.CourseID == id);
            if (hasStudents)
            {
                TempData["ErrorMessage"] = "Không thể xóa khóa học vì vẫn còn sinh viên đăng ký.";
                return RedirectToAction(nameof(Index));
            }

            _context.Courses.Remove(course);
            _context.SaveChanges();

            TempData["SuccessMessage"] = "Xóa khóa học thành công!";
            return RedirectToAction(nameof(Index));
        }
    }
}
