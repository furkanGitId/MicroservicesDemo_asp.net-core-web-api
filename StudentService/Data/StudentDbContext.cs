using Microsoft.EntityFrameworkCore;
using StudentService.Models;

namespace StudentService.Data
{
    public class StudentDbContext : DbContext
    {
        public StudentDbContext(DbContextOptions<StudentDbContext> options)
            : base(options)
        {
        }

        public DbSet<Student> Students { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Seed data
            modelBuilder.Entity<Student>().HasData(
                new Student
                {
                    Id = 1,
                    Name = "John Doe",
                    Email = "john@example.com",
                    EnrollmentDate = DateTime.UtcNow
                },
                new Student
                {
                    Id = 2,
                    Name = "Jane Smith",
                    Email = "jane@example.com",
                    EnrollmentDate = DateTime.UtcNow
                }
            );
        }
    }
}
