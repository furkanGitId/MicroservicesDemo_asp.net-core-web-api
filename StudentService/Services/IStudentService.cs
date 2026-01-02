using StudentService.DTOs;

namespace StudentService.Services
{
    public interface IStudentService
    {
        Task<IEnumerable<StudentDto>> GetAllStudentsAsync();
        Task<StudentDto?> GetStudentByIdAsync(int id);
        Task<StudentDto> CreateStudentAsync(CreateStudentDto createDto);
        Task<StudentDto?> UpdateStudentAsync(int id, UpdateStudentDto updateDto);
        Task<bool> DeleteStudentAsync(int id);
    }
}
