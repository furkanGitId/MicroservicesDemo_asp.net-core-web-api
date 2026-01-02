using CourseService.Data;
using CourseService.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Database
builder.Services.AddDbContext<CourseDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("CourseDB")));

// HttpClient for Student Service
builder.Services.AddHttpClient("StudentService", client =>
{
    client.BaseAddress = new Uri(builder.Configuration["Services:StudentServiceUrl"]
        ?? "https://localhost:7002");
    client.DefaultRequestHeaders.Add("Accept", "application/json");
});

// Services
builder.Services.AddScoped<ICourseService, CourseServiceImpl>();
builder.Services.AddScoped<IStudentServiceClient, StudentServiceClient>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
