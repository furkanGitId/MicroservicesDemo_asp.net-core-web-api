using System.ComponentModel.DataAnnotations;

namespace CourseService.Models
{
    public class Course
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(200)]
        public string Title { get; set; } = string.Empty;

        [MaxLength(1000)]
        public string Description { get; set; } = string.Empty;

        public int Credits { get; set; }

        // Store Student ID from Student Service
        public int InstructorId { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    }
}
