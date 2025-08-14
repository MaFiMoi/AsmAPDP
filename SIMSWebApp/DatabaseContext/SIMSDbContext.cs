using Microsoft.EntityFrameworkCore;
using SIMSWebApp.DatabaseContext.Entities;
using SIMSWebApp.Models;
using static SIMSWebApp.Models.Enrollments;

namespace SIMSWebApp.DatabaseContext
{
    public class SIMSDbContext : DbContext
    {
        public SIMSDbContext(DbContextOptions<SIMSDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Course> Courses { get; set; }   // Thêm DbSet cho bảng Courses
        public DbSet<Enrollment> Enrollments { get; set; }
        public DbSet<Class> Classes { get; set; }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().ToTable("Users");
            modelBuilder.Entity<User>().HasKey("UserID");
            modelBuilder.Entity<User>().HasIndex("Username").IsUnique();
            modelBuilder.Entity<User>().Property(u => u.Role).HasDefaultValue("Admin");

            // Cấu hình entity Course nếu cần
            modelBuilder.Entity<Course>().ToTable("Courses");
            modelBuilder.Entity<Course>().HasKey(c => c.CourseID);

            modelBuilder.Entity<Course>().Property(c => c.CourseCode).IsRequired().HasMaxLength(50);
            modelBuilder.Entity<Course>().Property(c => c.CourseName).IsRequired().HasMaxLength(100);
            modelBuilder.Entity<Course>().Property(c => c.Description).HasMaxLength(500);
            modelBuilder.Entity<Course>().Property(c => c.Department).HasMaxLength(100);
            modelBuilder.Entity<Course>().Property(c => c.CreatedAt).HasDefaultValueSql("GETDATE()");
        }
    }
}
