-- Схема бази даних для Варіанту 1: Освітня система університету (SQLite)

CREATE TABLE IF NOT EXISTS Teachers (
    ID INTEGER PRIMARY KEY AUTOINCREMENT,
    Name TEXT NOT NULL,
    Email TEXT NOT NULL UNIQUE,
    PasswordHash TEXT NOT NULL
);

CREATE TABLE IF NOT EXISTS Courses (
    ID INTEGER PRIMARY KEY AUTOINCREMENT,
    Name TEXT NOT NULL,
    Description TEXT,
    Semester TEXT NOT NULL,
    TeacherID INTEGER NOT NULL,
    FOREIGN KEY (TeacherID) REFERENCES Teachers(ID) ON DELETE CASCADE
);

CREATE TABLE IF NOT EXISTS Proposals (
    ID INTEGER PRIMARY KEY AUTOINCREMENT,
    TeacherID INTEGER NOT NULL,
    CourseName TEXT NOT NULL,
    ProposalType TEXT NOT NULL CHECK (ProposalType IN ('Create', 'Delete')),
    Status TEXT NOT NULL DEFAULT 'Pending',
    FOREIGN KEY (TeacherID) REFERENCES Teachers(ID) ON DELETE CASCADE
);

-- Тестові дані (пароль для обох викладачів: 123456)
INSERT INTO Teachers (Name, Email, PasswordHash) VALUES 
('Іван Петренко', 'petrenko@univ.edu.ua', '$2a$11$rBn2Z.9A9Y9Z9Z9Z9Z9Z9uK8J8J8J8J8J8J8J8J8J8J8J8J8J8J8G'),
('Олена Коваленко', 'kovalenko@univ.edu.ua', '$2a$11$rBn2Z.9A9Y9Z9Z9Z9Z9Z9uK8J8J8J8J8J8J8J8J8J8J8J8J8J8J8G');

INSERT INTO Courses (Name, Description, Semester, TeacherID) VALUES 
('Програмування на C#', 'Основи .NET та об''єктно-орієнтованого програмування', 'Осінній 2026', 1),
('Бази даних', 'Проєктування реляційних БД та мова SQL', 'Весняний 2026', 2);

INSERT INTO Proposals (TeacherID, CourseName, ProposalType, Status) VALUES 
(1, 'Розробка мобільних додатків на .NET MAUI', 'Create', 'Pending'),
(2, 'Застарілий курс HTML4', 'Delete', 'Approved');
