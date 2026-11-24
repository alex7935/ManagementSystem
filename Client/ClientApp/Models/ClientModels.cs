namespace ClientApp.Models;

public class LoginResponse
{
    public string Token { get; set; } = string.Empty;
    public int ID { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
}

public class CourseItem
{
    public int ID { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string Semester { get; set; } = string.Empty;
    public string TeacherName { get; set; } = string.Empty;
}