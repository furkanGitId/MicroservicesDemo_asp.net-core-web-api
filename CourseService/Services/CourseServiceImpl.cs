using CourseService.Data;
using CourseService.DTOs;
using CourseService.Models;
using Microsoft.EntityFrameworkCore;

namespace CourseService.Services
{
    public class CourseServiceImpl : ICourseService
    {
        private readonly CourseDbContext _context;
        private readonly IStudentServiceClient _studentServiceClient;
        private readonly ILogger<CourseServiceImpl> _logger;

        public CourseServiceImpl(
            CourseDbContext context,
            IStudentServiceClient studentServiceClient,
            ILogger<CourseServiceImpl> logger)
        {
            _context = context;
            _studentServiceClient = studentServiceClient;
            _logger = logger;
        }

        public async Task<IEnumerable<CourseDto>> GetAllCoursesAsync()
        {
            var courses = await _context.Courses.ToListAsync();
            var courseDtos = new List<CourseDto>();

            foreach (var course in courses)
            {
                var courseDto = await MapToDtoWithInstructorAsync(course);
                courseDtos.Add(courseDto);
            }

            return courseDtos;
        }

        public async Task<CourseDto?> GetCourseByIdAsync(int id)
        {
            var course = await _context.Courses.FindAsync(id);
            if (course == null) return null;

            return await MapToDtoWithInstructorAsync(course);
        }

        public async Task<CourseDto> CreateCourseAsync(CreateCourseDto createDto)
        {
            // Verify instructor exists in Student Service
            var instructor = await _studentServiceClient.GetStudentByIdAsync(createDto.InstructorId);
            if (instructor == null)
            {
                throw new InvalidOperationException(
                    $"Instructor with ID {createDto.InstructorId} not found in Student Service");
            }

            var course = new Course
            {
                Title = createDto.Title,
                Description = createDto.Description,
                Credits = createDto.Credits,
                InstructorId = createDto.InstructorId,
                CreatedDate = DateTime.UtcNow
            };

            _context.Courses.Add(course);
            await _context.SaveChangesAsync();

            return await MapToDtoWithInstructorAsync(course);
        }

        public async Task<CourseDto?> UpdateCourseAsync(int id, UpdateCourseDto updateDto)
        {
            var course = await _context.Courses.FindAsync(id);
            if (course == null) return null;

            // Verify new instructor exists if changed
            if (course.InstructorId != updateDto.InstructorId)
            {
                var instructor = await _studentServiceClient.GetStudentByIdAsync(updateDto.InstructorId);
                if (instructor == null)
                {
                    throw new InvalidOperationException(
                        $"Instructor with ID {updateDto.InstructorId} not found in Student Service");
                }
            }

            course.Title = updateDto.Title;
            course.Description = updateDto.Description;
            course.Credits = updateDto.Credits;
            course.InstructorId = updateDto.InstructorId;

            await _context.SaveChangesAsync();
            return await MapToDtoWithInstructorAsync(course);
        }

        public async Task<bool> DeleteCourseAsync(int id)
        {
            var course = await _context.Courses.FindAsync(id);
            if (course == null) return false;

            _context.Courses.Remove(course);
            await _context.SaveChangesAsync();
            return true;
        }

        private async Task<CourseDto> MapToDtoWithInstructorAsync(Course course)
        {
            var courseDto = new CourseDto
            {
                Id = course.Id,
                Title = course.Title,
                Description = course.Description,
                Credits = course.Credits,
                InstructorId = course.InstructorId,
                CreatedDate = course.CreatedDate
            };

            // Fetch instructor details from Student Service
            try
            {
                courseDto.Instructor = await _studentServiceClient.GetStudentByIdAsync(course.InstructorId);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Could not fetch instructor details for course {CourseId}", course.Id);
            }

            return courseDto;
        }
    }
}
