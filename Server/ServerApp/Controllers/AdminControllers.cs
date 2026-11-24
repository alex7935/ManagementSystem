using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ServerApp.Models;

namespace ServerApp.Controllers;

[Authorize(Roles = "Admin")]
public class TeachersController : Controller
{
    private readonly AppDbContext _db;
    public TeachersController(AppDbContext db) => _db = db;

    public async Task<IActionResult> Index() => View(await _db.Teachers.ToListAsync());

    public IActionResult Create() => View();

    [HttpPost]
    public async Task<IActionResult> Create(Teacher teacher, string plainPassword)
    {
        teacher.PasswordHash = BCrypt.Net.BCrypt.HashPassword(plainPassword);
        _db.Teachers.Add(teacher);
        await _db.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int id)
    {
        var teacher = await _db.Teachers.FindAsync(id);
        return teacher == null ? NotFound() : View(teacher);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(Teacher teacher, string? newPassword)
    {
        var existing = await _db.Teachers.FindAsync(teacher.ID);
        if (existing == null) return NotFound();
        existing.Name = teacher.Name;
        existing.Email = teacher.Email;
        if (!string.IsNullOrWhiteSpace(newPassword))
            existing.PasswordHash = BCrypt.Net.BCrypt.HashPassword(newPassword);
        await _db.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Delete(int id)
    {
        var teacher = await _db.Teachers.FindAsync(id);
        if (teacher != null) { _db.Teachers.Remove(teacher); await _db.SaveChangesAsync(); }
        return RedirectToAction(nameof(Index));
    }
}

[Authorize(Roles = "Admin")]
public class CoursesController : Controller
{
    private readonly AppDbContext _db;
    public CoursesController(AppDbContext db) => _db = db;

    public async Task<IActionResult> Index() => View(await _db.Courses.Include(c => c.Teacher).ToListAsync());

    public IActionResult Create()
    {
        ViewBag.Teachers = new SelectList(_db.Teachers, "ID", "Name");
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(Course course)
    {
        _db.Courses.Add(course);
        await _db.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int id)
    {
        var course = await _db.Courses.FindAsync(id);
        if (course == null) return NotFound();
        ViewBag.Teachers = new SelectList(_db.Teachers, "ID", "Name", course.TeacherID);
        return View(course);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(Course course)
    {
        _db.Courses.Update(course);
        await _db.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Delete(int id)
    {
        var course = await _db.Courses.FindAsync(id);
        if (course != null) { _db.Courses.Remove(course); await _db.SaveChangesAsync(); }
        return RedirectToAction(nameof(Index));
    }
}

[Authorize(Roles = "Admin")]
public class ProposalsController : Controller
{
    private readonly AppDbContext _db;
    public ProposalsController(AppDbContext db) => _db = db;

    public async Task<IActionResult> Index() => View(await _db.Proposals.Include(p => p.Teacher).ToListAsync());

    public IActionResult Create()
    {
        ViewBag.Teachers = new SelectList(_db.Teachers, "ID", "Name");
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(Proposal proposal)
    {
        _db.Proposals.Add(proposal);
        await _db.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int id)
    {
        var proposal = await _db.Proposals.FindAsync(id);
        if (proposal == null) return NotFound();
        ViewBag.Teachers = new SelectList(_db.Teachers, "ID", "Name", proposal.TeacherID);
        return View(proposal);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(Proposal proposal)
    {
        _db.Proposals.Update(proposal);
        await _db.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Delete(int id)
    {
        var proposal = await _db.Proposals.FindAsync(id);
        if (proposal != null) { _db.Proposals.Remove(proposal); await _db.SaveChangesAsync(); }
        return RedirectToAction(nameof(Index));
    }
}