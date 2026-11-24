using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ServerApp.Models;

namespace ServerApp.Controllers;

public class AuthController : Controller
{
    private readonly IConfiguration _config;
    private readonly AppDbContext _context;

    // Дані адміністратора зашиті в коді за умовою Завдання 4
    private const string AdminUsername = "admin";
    private const string AdminPassword = "admin123";

    public AuthController(IConfiguration config, AppDbContext context)
    {
        _config = config;
        _context = context;
    }

    [HttpGet("/auth/login")]
    public IActionResult Login()
    {
        return View();
    }

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
