using BCrypt.Net; // Thư viện BCrypt.Net-Next
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SIMSWebApp.DatabaseContext;
using SIMSWebApp.DatabaseContext.Entities;
using SIMSWebApp.Models;
using System;
using System.Linq;

namespace SIMSWebApp.Controllers
{
    [Authorize]
    public class StudentController : Controller
    {
        private readonly SIMSDbContext _context;

        public StudentController(SIMSDbContext context)
        {
            _context = context;
        }

        // Danh sách sinh viên
        [HttpGet]
        [Authorize(Roles = "Admin,Student,Faculty")]
        public IActionResult Index()
        {
            var students = _context.Students.ToList();
            return View(students);
        }

        // Form thêm sinh viên
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }

        // Xử lý thêm sinh viên
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public IActionResult Create(Student student)
        {
            if (ModelState.IsValid)
            {
                // 1. Tạo mật khẩu random
                var randomPassword = GenerateRandomPassword(10);

                // 2. Hash mật khẩu với BCrypt
                var hashedPassword = BCrypt.Net.BCrypt.HashPassword(randomPassword);

                // 3. Tạo user cho sinh viên
                var user = new User
                {
                    Username = student.Email,
                    PasswordHash = hashedPassword,
                    Role = "Student",
                    IsFirstLogin = true 
                };

                _context.Users.Add(user);
                _context.SaveChanges(); // Lưu để có UserID

                // 4. Gán UserID cho student
                student.UserID = user.UserID;
                _context.Students.Add(student);
                _context.SaveChanges();

                // 5. Thông báo (mật khẩu random hiển thị tạm cho Admin)
                TempData["SuccessMessage"] = $"Student created successfully. Username: {user.Username}, Password: {randomPassword}";

                return RedirectToAction(nameof(Index));
            }
            return View(student);
        }

        // Form chỉnh sửa
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult Edit(int id)
        {
            var student = _context.Students.FirstOrDefault(s => s.StudentID == id);
            if (student == null)
                return NotFound();

            return View(student);
        }

        // Lưu chỉnh sửa
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public IActionResult Edit(Student student)
        {
            if (ModelState.IsValid)
            {
                var existingStudent = _context.Students.FirstOrDefault(s => s.StudentID == student.StudentID);
                if (existingStudent == null)
                    return NotFound();

                existingStudent.FirstName = student.FirstName;
                existingStudent.LastName = student.LastName;
                existingStudent.Email = student.Email;
                existingStudent.Phone = student.Phone;
                existingStudent.Program = student.Program;
                existingStudent.DateOfBirth = student.DateOfBirth;
                existingStudent.Gender = student.Gender;
                existingStudent.Address = student.Address;
                existingStudent.EnrollmentDate = student.EnrollmentDate;

                _context.SaveChanges();

                TempData["SuccessMessage"] = "Student updated successfully.";
                return RedirectToAction(nameof(Index));
            }
            return View(student);
        }

        // Xác nhận xóa
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult Delete(int id)
        {
            var student = _context.Students.FirstOrDefault(s => s.StudentID == id);
            if (student == null)
                return NotFound();

            return View(student);
        }

        // Xóa sinh viên
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public IActionResult DeleteConfirmed(int id)
        {
            var student = _context.Students.FirstOrDefault(s => s.StudentID == id);
            if (student == null)
                return NotFound();

            // Xóa cả tài khoản User liên quan
            var user = _context.Users.FirstOrDefault(u => u.UserID == student.UserID);
            if (user != null)
            {
                _context.Users.Remove(user);
            }

            _context.Students.Remove(student);
            _context.SaveChanges();

            TempData["SuccessMessage"] = "Student deleted successfully.";
            return RedirectToAction(nameof(Index));
        }

        // Hàm tạo mật khẩu random
        private string GenerateRandomPassword(int length)
        {
            const string validChars = "ABCDEFGHJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var random = new Random();
            return new string(Enumerable.Repeat(validChars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}
