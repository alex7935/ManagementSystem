using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ServerApp.Models;

namespace ServerApp.Controllers;

public record ApiLoginDto(string Email, string Password);

public class AuthController : Controller
{
    private readonly IConfiguration _config;
    private readonly AppDbContext _context;

    private const string AdminUsername = "admin";
    private const string AdminPassword = "admin123";

    public AuthController(IConfiguration config, AppDbContext context)
    {
        _config = config;
        _context = context;
    }

    [HttpGet("/auth/login")]
    public IActionResult Login() => View();

    [HttpPost("/auth/login")]
    public IActionResult Login(string username, string password)
    {
        if (username == AdminUsername && password == AdminPassword)
        {
            var token = GenerateJwtToken(AdminUsername, "Admin", 0);
            Response.Cookies.Append("AdminJwtToken", token, new CookieOptions
            {
                HttpOnly = true,
                SameSite = SameSiteMode.Lax,
                Expires = DateTimeOffset.UtcNow.AddHours(4)
            });
            return RedirectToAction("Index", "Courses");
        }

        ViewBag.Error = "Невірний логін або пароль адміністратора";
        return View();
    }

    [HttpGet("/auth/logout")]
    public IActionResult Logout()
    {
        Response.Cookies.Delete("AdminJwtToken");
        return Redirect("/auth/login");
    }

    // Ендпоінт аутентифікації користувачів (викладачів із БД) для мобільного додатку
    [HttpPost("/api/auth/login")]
    public async Task<IActionResult> ApiLogin([FromBody] ApiLoginDto dto)
    {
        var teacher = await _context.Teachers.FirstOrDefaultAsync(t => t.Email == dto.Email);
        if (teacher == null || !BCrypt.Net.BCrypt.Verify(dto.Password, teacher.PasswordHash))
        {
            return Unauthorized(new { message = "Невірний email або пароль" });
        }

        var token = GenerateJwtToken(teacher.Email, "Teacher", teacher.ID);
        return Ok(new { token, teacher.ID, teacher.Name, teacher.Email });
    }

    private string GenerateJwtToken(string name, string role, int userId)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(ClaimTypes.Name, name),
            new Claim(ClaimTypes.Role, role),
            new Claim("UserId", userId.ToString())
        };

        var token = new JwtSecurityToken(
            issuer: _config["Jwt:Issuer"],
            audience: _config["Jwt:Audience"],
            claims: claims,
            expires: DateTime.Now.AddHours(4),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}