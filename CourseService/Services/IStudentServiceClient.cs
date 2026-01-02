using CourseService.DTOs;

namespace CourseService.Services
{
    public interface IStudentServiceClient
    {
        Task<StudentDto?> GetStudentByIdAsync(int studentId);
    }
}
