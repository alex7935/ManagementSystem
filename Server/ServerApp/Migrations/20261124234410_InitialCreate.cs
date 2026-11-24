using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ServerApp.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Teachers",
                columns: table => new
                {
                    ID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Email = table.Column<string>(type: "TEXT", nullable: false),
                    PasswordHash = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Teachers", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Courses",
                columns: table => new
                {
                    ID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: true),
                    Semester = table.Column<string>(type: "TEXT", nullable: false),
                    TeacherID = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Courses", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Courses_Teachers_TeacherID",
                        column: x => x.TeacherID,
                        principalTable: "Teachers",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Proposals",
                columns: table => new
                {
                    ID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    TeacherID = table.Column<int>(type: "INTEGER", nullable: false),
                    CourseName = table.Column<string>(type: "TEXT", nullable: false),
                    ProposalType = table.Column<string>(type: "TEXT", nullable: false),
                    Status = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Proposals", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Proposals_Teachers_TeacherID",
                        column: x => x.TeacherID,
                        principalTable: "Teachers",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Teachers",
                columns: new[] { "ID", "Email", "Name", "PasswordHash" },
                values: new object[,]
                {
                    { 1, "petrenko@univ.edu.ua", "Іван Петренко", "$2a$11$llYTOKJ5Oh73cigwQ09Yn.SZC8I6ZZW4jaAZcRLmHFdGgzU0zsfYW" },
                    { 2, "kovalenko@univ.edu.ua", "Олена Коваленко", "$2a$11$llYTOKJ5Oh73cigwQ09Yn.SZC8I6ZZW4jaAZcRLmHFdGgzU0zsfYW" }
                });

            migrationBuilder.InsertData(
                table: "Courses",
                columns: new[] { "ID", "Description", "Name", "Semester", "TeacherID" },
                values: new object[,]
                {
                    { 1, "Основи .NET та ООП", "Програмування на C#", "Осінній 2026", 1 },
                    { 2, "Проєктування реляційних БД та SQL", "Бази даних", "Весняний 2026", 2 }
                });

            migrationBuilder.InsertData(
                table: "Proposals",
                columns: new[] { "ID", "CourseName", "ProposalType", "Status", "TeacherID" },
                values: new object[,]
                {
                    { 1, "Розробка мобільних додатків на .NET MAUI", "Create", "Pending", 1 },
                    { 2, "Застарілий курс HTML4", "Delete", "Approved", 2 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Courses_TeacherID",
                table: "Courses",
                column: "TeacherID");

            migrationBuilder.CreateIndex(
                name: "IX_Proposals_TeacherID",
                table: "Proposals",
                column: "TeacherID");

            migrationBuilder.CreateIndex(
                name: "IX_Teachers_Email",
                table: "Teachers",
                column: "Email",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Courses");

            migrationBuilder.DropTable(
                name: "Proposals");

            migrationBuilder.DropTable(
                name: "Teachers");
        }
    }
}
