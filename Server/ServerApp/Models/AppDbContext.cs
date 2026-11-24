using Microsoft.EntityFrameworkCore;

namespace ServerApp.Models;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Teacher> Teachers => Set<Teacher>();
    public DbSet<Course> Courses => Set<Course>();
    public DbSet<Proposal> Proposals => Set<Proposal>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Індекси для оптимізації (знадобиться і в Завданні 11)
        modelBuilder.Entity<Teacher>().HasIndex(t => t.Email).IsUnique();
        modelBuilder.Entity<Course>().HasIndex(c => c.TeacherID);

        // Початкові тестові дані (пароль для викладачів: 123456)
        string defaultHash = BCrypt.Net.BCrypt.HashPassword("123456");

        modelBuilder.Entity<Teacher>().HasData(
            new Teacher { ID = 1, Name = "Іван Петренко", Email = "petrenko@univ.edu.ua", PasswordHash = defaultHash },
            new Teacher { ID = 2, Name = "Олена Коваленко", Email = "kovalenko@univ.edu.ua", PasswordHash = defaultHash }
        );

        modelBuilder.Entity<Course>().HasData(
            new Course { ID = 1, Name = "Програмування на C#", Description = "Основи .NET та ООП", Semester = "Осінній 2026", TeacherID = 1 },
            new Course { ID = 2, Name = "Бази даних", Description = "Проєктування реляційних БД та SQL", Semester = "Весняний 2026", TeacherID = 2 }
        );

        modelBuilder.Entity<Proposal>().HasData(
            new Proposal { ID = 1, TeacherID = 1, CourseName = "Розробка мобільних додатків на .NET MAUI", ProposalType = "Create", Status = "Pending" },
            new Proposal { ID = 2, TeacherID = 2, CourseName = "Застарілий курс HTML4", ProposalType = "Delete", Status = "Approved" }
        );
    }
}
