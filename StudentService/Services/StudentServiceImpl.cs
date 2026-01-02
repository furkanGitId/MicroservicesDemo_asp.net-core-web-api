using Microsoft.EntityFrameworkCore;
using StudentService.Data;
using StudentService.DTOs;
using StudentService.Models;

namespace StudentService.Services
{
    public class StudentServiceImpl : IStudentService
    {
        private readonly StudentDbContext _context;

        public StudentServiceImpl(StudentDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<StudentDto>> GetAllStudentsAsync()
        {
            var students = await _context.Students.ToListAsync();
            return students.Select(s => MapToDto(s));
        }

        public async Task<StudentDto?> GetStudentByIdAsync(int id)
        {
            var student = await _context.Students.FindAsync(id);
            return student != null ? MapToDto(student) : null;
        }

        public async Task<StudentDto> CreateStudentAsync(CreateStudentDto createDto)
        {
            var student = new Student
            {
                Name = createDto.Name,
                Email = createDto.Email,
                EnrollmentDate = DateTime.UtcNow
            };

            _context.Students.Add(student);
            await _context.SaveChangesAsync();

            return MapToDto(student);
        }

        public async Task<StudentDto?> UpdateStudentAsync(int id, UpdateStudentDto updateDto)
        {
            var student = await _context.Students.FindAsync(id);
            if (student == null) return null;

            student.Name = updateDto.Name;
            student.Email = updateDto.Email;

            await _context.SaveChangesAsync();
            return MapToDto(student);
        }

        public async Task<bool> DeleteStudentAsync(int id)
        {
            var student = await _context.Students.FindAsync(id);
            if (student == null) return false;

            _context.Students.Remove(student);
            await _context.SaveChangesAsync();
            return true;
        }

        private static StudentDto MapToDto(Student student)
        {
            return new StudentDto
            {
                Id = student.Id,
                Name = student.Name,
                Email = student.Email,
                EnrollmentDate = student.EnrollmentDate
            };
        }
    }
}
