using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SIMSWebApp.DatabaseContext;
using SIMSWebApp.DatabaseContext.Entities;
using System.Linq;
using System.Threading.Tasks;

namespace SIMSWebApp.Controllers
{
    [Authorize] // Áp dụng chung, sau đó phân quyền cụ thể cho từng action
    public class ClassController : Controller
    {
        private readonly SIMSDbContext _context;

        public ClassController(SIMSDbContext context)
        {
            _context = context;
        }

        // GET: Class
        [Authorize(Roles = "Admin,Faculty,Student")]
        public async Task<IActionResult> Index()
        {
            var classes = await _context.Classes.ToListAsync();
            return View(classes);
        }

        // GET: Class/Create
        [Authorize(Roles = "Admin,Faculty")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Class/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Faculty")]
        public async Task<IActionResult> Create(Class model)
        {
            if (model.EndDate.HasValue && model.StartDate.HasValue && model.EndDate < model.StartDate)
            {
                ModelState.AddModelError("EndDate", "End Date không được nhỏ hơn Start Date");
            }

            if (ModelState.IsValid)
            {
                _context.Add(model);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Tạo lớp thành công!";
                return RedirectToAction(nameof(Index));
            }

            return View(model);
        }

        // GET: Class/Edit/5
        [Authorize(Roles = "Admin,Faculty")]
        public async Task<IActionResult> Edit(int id)
        {
            var classEntity = await _context.Classes.FindAsync(id);
            if (classEntity == null)
            {
                return NotFound();
            }
            return View(classEntity);
        }

        // POST: Class/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Faculty")]
        public async Task<IActionResult> Edit(int id, Class model)
        {
            if (id != model.ClassId) return NotFound();

            if (model.EndDate.HasValue && model.StartDate.HasValue && model.EndDate < model.StartDate)
            {
                ModelState.AddModelError("EndDate", "End Date không được nhỏ hơn Start Date");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(model);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Cập nhật lớp thành công!";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Classes.Any(c => c.ClassId == id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        // GET: Class/Delete/5
        [Authorize(Roles = "Admin,Faculty")]
        public async Task<IActionResult> Delete(int id)
        {
            var classEntity = await _context.Classes.FindAsync(id);
            if (classEntity == null)
            {
                return NotFound();
            }
            return View(classEntity);
        }

        // POST: Class/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Faculty")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var classEntity = await _context.Classes
                .Include(c => c.Students)
                .FirstOrDefaultAsync(c => c.ClassId == id);

            if (classEntity == null)
            {
                return NotFound();
            }

            if (classEntity.Students.Any())
            {
                TempData["ErrorMessage"] = "Không thể xóa lớp vì vẫn còn sinh viên trong lớp.";
                return RedirectToAction(nameof(Index));
            }

            _context.Classes.Remove(classEntity);
            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = "Xóa lớp thành công!";
            return RedirectToAction(nameof(Index));
        }

        // GET: Class/Details/5 - Xem danh sách sinh viên trong lớp
        [Authorize(Roles = "Admin,Faculty,Student")]
        public async Task<IActionResult> Details(int id)
        {
            var cls = await _context.Classes
                .Include(c => c.Students)
                .FirstOrDefaultAsync(c => c.ClassId == id);

            if (cls == null)
            {
                TempData["ErrorMessage"] = "Lớp không tồn tại.";
                return RedirectToAction(nameof(Index));
            }

            return View(cls);
        }

        // GET: Class/AddStudent
        [HttpGet]
        [Authorize(Roles = "Admin,Faculty")]
        public IActionResult AddStudent(int classId)
        {
            var cls = _context.Classes.Find(classId);
            if (cls == null)
            {
                TempData["ErrorMessage"] = "Lớp không tồn tại.";
                return RedirectToAction(nameof(Index));
            }

            ViewBag.ClassId = classId;
            ViewBag.Students = _context.Students
                .Where(s => s.ClassId == null)
                .ToList();

            return View();
        }

        // POST: Class/AddStudent
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Faculty")]
        public IActionResult AddStudent(int classId, int studentId)
        {
            var student = _context.Students.Find(studentId);
            if (student == null)
            {
                TempData["ErrorMessage"] = "Sinh viên không tồn tại.";
                return RedirectToAction("Details", new { id = classId });
            }

            student.ClassId = classId;
            _context.SaveChanges();

            TempData["SuccessMessage"] = "Thêm sinh viên thành công.";
            return RedirectToAction("Details", new { id = classId });
        }

        // POST: Class/RemoveStudent
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Faculty")]
        public IActionResult RemoveStudent(int classId, int studentId)
        {
            var student = _context.Students.Find(studentId);
            if (student != null && student.ClassId == classId)
            {
                student.ClassId = null;
                _context.SaveChanges();
                TempData["SuccessMessage"] = "Xóa sinh viên khỏi lớp thành công.";
            }
            else
            {
                TempData["ErrorMessage"] = "Không tìm thấy sinh viên trong lớp.";
            }

            return RedirectToAction("Details", new { id = classId });
        }
    }
}
