using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ServerApp.Models;

namespace ServerApp.Controllers;

[ApiController]
[Route("api/courses")]
[Authorize]
public class CoursesApiController : ControllerBase
{
    private readonly AppDbContext _db;
    public CoursesApiController(AppDbContext db) => _db = db;

    [HttpGet]
    public async Task<IActionResult> GetCourses()
    {
        var userIdClaim = User.FindFirst("UserId")?.Value;
        var query = _db.Courses.Include(c => c.Teacher).AsQueryable();

        if (int.TryParse(userIdClaim, out int teacherId) && teacherId > 0)
        {
            query = query.Where(c => c.TeacherID == teacherId);
        }

        var list = await query.Select(c => new
        {
            c.ID,
            c.Name,
            c.Description,
            c.Semester,
            TeacherName = c.Teacher != null ? c.Teacher.Name : ""
        }).ToListAsync();

        return Ok(list);
    }
}

public record ProposalCreateDto(string CourseName, string ProposalType);

[ApiController]
[Route("api/proposals")]
[Authorize]
public class ProposalsApiController : ControllerBase
{
    private readonly AppDbContext _db;
    public ProposalsApiController(AppDbContext db) => _db = db;

    [HttpPost]
    public async Task<IActionResult> CreateProposal([FromBody] ProposalCreateDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.CourseName) || (dto.ProposalType != "Create" && dto.ProposalType != "Delete"))
            return BadRequest(new { message = "Вкажіть назву курсу та тип (Create або Delete)" });

        var userIdClaim = User.FindFirst("UserId")?.Value;
        int teacherId = int.TryParse(userIdClaim, out int id) && id > 0 ? id : 1;

        var proposal = new Proposal
        {
            TeacherID = teacherId,
            CourseName = dto.CourseName,
            ProposalType = dto.ProposalType,
            Status = "Pending"
        };

        _db.Proposals.Add(proposal);
        await _db.SaveChangesAsync();

        return Ok(new { proposal.ID, proposal.CourseName, proposal.ProposalType, proposal.Status });
    }
}