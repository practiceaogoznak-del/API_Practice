using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;

var builder = WebApplication.CreateBuilder(args);

// Логирование
builder.Logging.AddConsole();
builder.Logging.AddDebug();

// Добавление сервисов ДО вызова builder.Build()
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "Zadelka API", Version = "v1" });
});

// Подключение к БД — должно быть до builder.Build()
var connectionString = Environment.GetEnvironmentVariable("DATABASE_URL") ??
                      builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<ZadelkaContext>(options =>
    options.UseNpgsql(connectionString));

// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        policy =>
        {
            policy.AllowAnyOrigin()
                  .AllowAnyMethod()
                  .AllowAnyHeader();
        });
});

// Строим приложение
var app = builder.Build();

// Middleware (после builder.Build())
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Zadelka API v1"));
}

app.UseHttpsRedirection();
app.UseCors("AllowAll");
app.UseAuthorization();
app.MapControllers();

// Запуск приложения
var port = Environment.GetEnvironmentVariable("PORT") ?? "8080";
app.Run($"http://0.0.0.0:{port}");