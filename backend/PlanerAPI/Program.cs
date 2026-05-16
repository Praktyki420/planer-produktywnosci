using Microsoft.EntityFrameworkCore;
using PlanerAPI.Data;

var builder = WebApplication.CreateBuilder(args);

// Baza danych SQLite
builder.Services.AddDbContext<AppDbContext>(opt =>
    opt.UseSqlite("Data Source=planer.db"));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApi();

// Zezwól na zapytania z Angular (CORS)
builder.Services.AddCors(opt =>
    opt.AddDefaultPolicy(p => p.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader()));

var app = builder.Build();

app.MapOpenApi();

app.UseCors();
app.MapControllers();

// Stwórz bazę danych przy starcie jeśli nie istnieje
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.EnsureCreated();
}

app.Run();