using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PlanerAPI.Data;
using PlanerAPI.Models;
using System.Security.Cryptography;
using System.Text;

namespace PlanerAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly AppDbContext _db;
    public UsersController(AppDbContext db) { _db = db; }

    // POST /api/users/register
    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterDto dto)
    {
        if (await _db.Users.AnyAsync(u => u.Email == dto.Email))
            return BadRequest("Email już istnieje");

        var user = new User
        {
            Username     = dto.Username,
            Email        = dto.Email,
            PasswordHash = HashPassword(dto.Password)
        };
        _db.Users.Add(user);
        await _db.SaveChangesAsync();
        return Ok(new { user.Id, user.Username, user.Email });
    }

    // POST /api/users/login
    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDto dto)
    {
        var user = await _db.Users.FirstOrDefaultAsync(u => u.Email == dto.Email);
        if (user == null || user.PasswordHash != HashPassword(dto.Password))
            return Unauthorized("Błędny email lub hasło");

        return Ok(new { user.Id, user.Username, user.Email });
    }

    private static string HashPassword(string password)
    {
        var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(password));
        return Convert.ToHexString(bytes);
    }
}

// Pomocnicze klasy DTO (Data Transfer Object)
public record RegisterDto(string Username, string Email, string Password);
public record LoginDto(string Email, string Password);