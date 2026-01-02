using CourseService.DTOs;
using System.Text.Json;

namespace CourseService.Services
{
    public class StudentServiceClient : IStudentServiceClient
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<StudentServiceClient> _logger;

        public StudentServiceClient(
            IHttpClientFactory httpClientFactory,
            ILogger<StudentServiceClient> logger)
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;
        }

        public async Task<StudentDto?> GetStudentByIdAsync(int studentId)
        {
            try
            {
                var client = _httpClientFactory.CreateClient("StudentService");
                var response = await client.GetAsync($"/api/students/{studentId}");

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogWarning(
                        "Failed to get student {StudentId}. Status: {StatusCode}",
                        studentId,
                        response.StatusCode);
                    return null;
                }

                var content = await response.Content.ReadAsStringAsync();
                var student = JsonSerializer.Deserialize<StudentDto>(content,
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

                return student;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error calling Student Service for student {StudentId}", studentId);
                return null;
            }
        }
    }
}
