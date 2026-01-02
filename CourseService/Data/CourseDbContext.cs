using CourseService.Models;
using Microsoft.EntityFrameworkCore;

namespace CourseService.Data
{
    public class CourseDbContext : DbContext
    {
        public CourseDbContext(DbContextOptions<CourseDbContext> options)
            : base(options)
        {
        }

        public DbSet<Course> Courses { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Seed data
            modelBuilder.Entity<Course>().HasData(
                new Course
                {
                    Id = 1,
                    Title = "Introduction to C#",
                    Description = "Learn C# programming basics",
                    Credits = 3,
                    InstructorId = 1,
                    CreatedDate = DateTime.UtcNow
                },
                new Course
                {
                    Id = 2,
                    Title = "Advanced ASP.NET Core",
                    Description = "Master ASP.NET Core development",
                    Credits = 4,
                    InstructorId = 2,
                    CreatedDate = DateTime.UtcNow
                }
            );
        }
    }

}
