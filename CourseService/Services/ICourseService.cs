using CourseService.DTOs;

namespace CourseService.Services
{
    public interface ICourseService
    {
        Task<IEnumerable<CourseDto>> GetAllCoursesAsync();
        Task<CourseDto?> GetCourseByIdAsync(int id);
        Task<CourseDto> CreateCourseAsync(CreateCourseDto createDto);
        Task<CourseDto?> UpdateCourseAsync(int id, UpdateCourseDto updateDto);
        Task<bool> DeleteCourseAsync(int id);
    }
}
