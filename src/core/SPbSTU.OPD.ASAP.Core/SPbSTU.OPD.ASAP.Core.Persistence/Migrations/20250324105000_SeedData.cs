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
        values ('test_subject');

        insert into courses (title, subject_id, github_organization) 
        values ('opd-asap-test', 1, 'https://github.com/opd-asap-test');

        insert into assignments (course_id, title, description, max_points, due_to) 
        values (1, 'test_assignment', 'Тестовое задание для проверки функционала на демо.', 100, now())
             , (1, 'test', 'Тестовое задание', 100, now());

        insert into groups (id)
        values (513090330002);

        insert into users (name, login, password, email, role, github_username) 
        values ('Александр_Градов', 'gradov.al', 'password', 'gradov.al@edu.spbstu.ru', 'mentor', 'Shiyonger')
             , ('Тимур_Хасанов', 'hasanov.ta', 'password', 'hasanov.ta@edu.spbstu.ru', 'student', 'HasanovTimur9');

        insert into students (group_id, user_id) 
        values (513090330002, 2)
             , (513090330002, 1);

        insert into mentors (department, user_id) 
        values ('ИКНК', 1);

        insert into repositories (student_id, assignment_id, link)
        values (1, 1, 'https://github.com/opd-asap-test/test_assignment')
             , (1, 2, 'https://github.com/opd-asap-test/test');

        insert into subject_course_groups (course_id, group_id, mentor_id) 
        values (1, 513090330002, 1);

        insert into student_courses (user_id, course_id, is_invited) 
        values (1, 1, false)
             , (2, 1, false);
        """;

    protected override string GetDownSql(IServiceProvider services) =>
        """
        delete from student_courses;
        delete from subject_course_groups;
        delete from repositories;
        delete from mentors;
        delete from students;
        delete from users;
        delete from groups;
        delete from assignments;
        delete from courses;
        delete from subjects;
        delete from actions;
        """;
}