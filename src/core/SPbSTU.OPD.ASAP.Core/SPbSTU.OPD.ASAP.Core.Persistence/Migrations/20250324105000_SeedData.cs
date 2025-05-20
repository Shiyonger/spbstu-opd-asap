using FluentMigrator;
using SPbSTU.OPD.ASAP.Core.Persistence.Common;

namespace SPbSTU.OPD.ASAP.Core.Persistence.Migrations;

[Migration(202503241050, "Seed data migration")]
public class SeedData : SqlMigration
{
    protected override string GetUpSql(IServiceProvider services) =>
        """
        insert into actions (id, name)
        values (0, 'Create')
             , (1, 'Update')
             , (2, 'Delete');

        insert into subjects (title)
        values ('Технологии_ООП')
             , ('Вычислительная_математика')
             , ('Алгоритмизация_и_программирование');

        insert into courses (title, subject_id, github_organization, google_spreadsheet) 
        values ('ТООП-2024-осень', 1, 'test', 'test')
             , ('Вычислительная_математика-2024-осень', 2, 'test', 'test')
             , ('Вычислительная_математика-2025-весна', 2, 'test', 'test')
             , ('АиП-2023-осень', 3, 'test', 'test')
             , ('АиП-2024-весна', 3, 'test', 'test');

        insert into assignments (course_id, title, description, max_points, due_to, spreadsheet_id, spreadsheet_link) 
        values (1, 'Наследование_в_C++', '', 100, now(), 'test', 'test')
             , (1, 'Лямбда-функции', '', 100, now(), 'test', 'test')
             , (2, 'Сплайн-интерполяция', '', 100, now(), 'test', 'test')
             , (2, 'Линейная_алгебра', '', 100, now(), 'test', 'test')
             , (3, 'Методы_оптимизации', '', 100, now(), 'test', 'test')
             , (4, 'Курьер_в_газпроме', '', 100, now(), 'test', 'test')
             , (5, 'Ханойские_башни', '', 100, now(), 'test', 'test');

        insert into groups (id)
        values (513090330002);

        insert into users (name, login, password, email, role, github_username) 
        values ('Александр_Градов', 'gradov.al', 'password', 'gradov.al@edu.spbstu.ru', 'mentor', 'Shiyonger')
             , ('Тимур_Хасанов', 'hasanov.ta', 'password', 'hasanov.ta@edu.spbstu.ru', 'student', 'HasanovTimur9')
             , ('Илья_Болтышев', 'boltyshev.ip', 'password', 'boltyshev.ip@edu.spbstu.ru', 'student', 'Ilya')
             , ('Елизавета_Ковкова', 'kovkova.ea', 'password', 'kovkova.ea@edu.spbstu.ru', 'student', 'zkmmhn')
             , ('Анна_Малышева', 'malysheva6.aa', 'password', 'malysheva6.aa@edu.spbstu.ru', 'student', 'MalyAnya');

        insert into students (group_id, user_id) 
        values (513090330002, 2)
             , (513090330002, 3)
             , (513090330002, 4)
             , (513090330002, 5);

        insert into mentors (department, user_id) 
        values ('ИКНК', 1);

        insert into repositories (student_id, assignment_id, link)
        values (1, 1, 'test')
             , (1, 2, 'test')
             , (1, 3, 'test')
             , (1, 4, 'test')
             , (1, 5, 'test')
             , (1, 6, 'test')
             , (1, 7, 'test')
             , (2, 1, 'test')
             , (2, 2, 'test')
             , (2, 3, 'test')
             , (2, 4, 'test')
             , (2, 5, 'test')
             , (2, 6, 'test')
             , (2, 7, 'test')
             , (3, 1, 'test')
             , (3, 2, 'test')
             , (3, 3, 'test')
             , (3, 4, 'test')
             , (3, 5, 'test')
             , (3, 6, 'test')
             , (3, 7, 'test')
             , (4, 1, 'test')
             , (4, 2, 'test')
             , (4, 3, 'test')
             , (4, 4, 'test')
             , (4, 5, 'test')
             , (4, 6, 'test')
             , (4, 7, 'test');

        insert into subject_course_groups (course_id, group_id, mentor_id) 
        values (1, 513090330002, 1)
             , (2, 513090330002, 1)
             , (3, 513090330002, 1)
             , (4, 513090330002, 1)
             , (5, 513090330002, 1);

        insert into google_positions (cell, spreadsheet_id)
        values ('1', '1')
             , ('1', '1')
             , ('1', '1')
             , ('1', '1')
             , ('1', '1')
             , ('1', '1')
             , ('1', '1')
             , ('1', '1')
             , ('1', '1')
             , ('1', '1')
             , ('1', '1');

        insert into google (student_id, course_id, assignment_id, assignment_position_id, student_position_id) 
        values (1, 1, 1, 5, 1)
             , (1, 1, 2, 6, 1)
             , (2, 1, 1, 5, 2)
             , (2, 1, 2, 6, 2)
             , (3, 1, 1, 5, 3)
             , (3, 1, 2, 6, 3)
             , (4, 1, 1, 5, 4)
             , (4, 1, 2, 6, 4)
             , (1, 1, 1, 5, 1)
             , (1, 1, 2, 6, 1)
             , (2, 1, 1, 5, 2)
             , (2, 1, 2, 6, 2)
             , (3, 1, 1, 5, 3)
             , (3, 1, 2, 6, 3)
             , (4, 1, 1, 5, 4)
             , (4, 1, 2, 6, 4)
             , (1, 2, 3, 7, 1)
             , (1, 2, 4, 8, 1)
             , (2, 2, 3, 7, 2)
             , (2, 2, 4, 8, 2)
             , (3, 2, 3, 7, 3)
             , (3, 2, 4, 8, 3)
             , (4, 2, 3, 7, 4)
             , (4, 2, 4, 8, 4)
             , (1, 3, 5, 9, 1)
             , (2, 3, 5, 9, 2)
             , (3, 3, 5, 9, 3)
             , (4, 3, 5, 9, 4)
             , (1, 4, 6, 10, 1)
             , (2, 4, 6, 10, 2)
             , (3, 4, 6, 10, 3)
             , (4, 4, 6, 10, 4)
             , (1, 5, 7, 11, 1)
             , (2, 5, 7, 11, 2)
             , (3, 5, 7, 11, 3)
             , (4, 5, 7, 11, 4);

        insert into student_courses (student_id, course_id, is_invited) 
        values (1, 1, false)
             , (1, 2, false)
             , (1, 3, false)
             , (1, 4, false)
             , (1, 5, false)
             , (2, 1, false)
             , (2, 2, false)
             , (2, 3, false)
             , (2, 4, false)
             , (2, 5, false)
             , (3, 1, false)
             , (3, 2, false)
             , (3, 3, false)
             , (3, 4, false)
             , (3, 5, false)
             , (4, 1, false)
             , (4, 2, false)
             , (4, 3, false)
             , (4, 4, false)
             , (4, 5, false);
        """;

    protected override string GetDownSql(IServiceProvider services) =>
        """
        delete from actions
        """;
}