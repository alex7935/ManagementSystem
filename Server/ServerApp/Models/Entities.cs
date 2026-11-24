using System.ComponentModel.DataAnnotations;

namespace ServerApp.Models;

public class Teacher
{
    public int ID { get; set; }
    [Required]
    public string Name { get; set; } = string.Empty;
    [Required, EmailAddress]
    public string Email { get; set; } = string.Empty;
    [Required]
    public string PasswordHash { get; set; } = string.Empty;

    public ICollection<Course> Courses { get; set; } = new List<Course>();
    public ICollection<Proposal> Proposals { get; set; } = new List<Proposal>();
}

public class Course
{
    public int ID { get; set; }
    [Required]
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    [Required]
    public string Semester { get; set; } = string.Empty;

    public int TeacherID { get; set; }
    public Teacher? Teacher { get; set; }
}

public class Proposal
{
    public int ID { get; set; }
    public int TeacherID { get; set; }
    public Teacher? Teacher { get; set; }

    [Required]
    public string CourseName { get; set; } = string.Empty;
    [Required]
    public string ProposalType { get; set; } = "Create"; // Create або Delete
    [Required]
    public string Status { get; set; } = "Pending"; // Pending, Approved, Rejected
}
