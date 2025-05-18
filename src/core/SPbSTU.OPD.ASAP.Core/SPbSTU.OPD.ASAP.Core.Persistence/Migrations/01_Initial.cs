using FluentMigrator;
using SPbSTU.OPD.ASAP.Core.Persistence.Common;

namespace SPbSTU.OPD.ASAP.Core.Persistence.Migrations;

[Migration(1, "Initial migration")]
public class Initial : SqlMigration
{
    protected override string GetUpSql(IServiceProvider services) =>
        """
        create table subjects (
            id bigserial primary key,
            title varchar not null
        );
        
        create table courses (
            id bigserial primary key,
            title varchar not null,
            subject_id bigint references subjects (id),
            github_organization varchar not null,
            google_spreadsheet varchar not null
        );
        
        create table assignments (
            id bigserial primary key,
            course_id bigint references courses (id),
            title varchar not null,
            description varchar,
            max_points int not null,
            due_to timestamptz not null
        );
        
        create table groups (
            id bigint primary key
        );
        
        create table users (
            id bigserial primary key,
            name varchar not null,
            login varchar not null,
            password varchar not null,
            email varchar not null,
            role varchar not null,
            github_username varchar not null
        );
        
        create table students (
            id bigserial primary key,
            group_id bigint references groups (id),
            user_id bigint references users (id)
        );
        
        create table mentors (
            id bigserial primary key,
            department varchar not null,
            user_id bigint references users (id)
        );
        
        create table subject_course_groups (
            id bigserial primary key,
            course_id bigint references courses (id),
            group_id bigint references groups (id),
            mentor_id bigint references mentors (id)
        );
        
        create table google_positions (
            id bigserial primary key,
            cell varchar not null,
            spreadsheet_id varchar not null
        );
        
        create table google (
            id bigserial primary key,
            student_id bigint references students (id),
            course_id bigint references courses (id),
            assignment_id bigint references assignments (id),
            assignment_position_id bigint references google_positions (id),
            student_position_id bigint references google_positions(id)
        );
        
        create table repositories (
            id bigserial primary key,
            student_id bigint references students (id),
            assignment_id bigint references assignments (id),
            link varchar not null
        );

        create table submissions (
            id bigserial primary key,
            student_id bigint references students (id),
            assignment_id bigint references assignments (id),
            repository_id bigint references repositories (id),
            created_at timestamptz not null,
            updated_at timestamptz not null
        );

        create table student_courses (
            id bigserial primary key,
            student_id bigint references students (id),
            course_id bigint references courses (id),
            is_invited boolean not null
        );

        create table outbox_points (
            id bigserial primary key,
            points integer not null,
            date timestamp with time zone,
            course_id bigint references courses (id),
            student_position_cell varchar not null,
            student_position_spreadsheet_id varchar not null,
            assignment_position_cell varchar not null,
            assignment_position_spreadsheet_id varchar not null,
            is_sent boolean not null
        );

        create table outbox_queue (
            id bigserial primary key,
            link varchar not null,
            mentor_id bigint references mentors (id),
            assignment_id bigint references assignments (id),
            submission_id bigint references submissions (id),
            action integer not null,
            is_sent boolean not null
        );

        create table actions (
            id integer unique primary key,
            name varchar not null
        );
        """;

    protected override string GetDownSql(IServiceProvider services) =>
        """
        drop table actions;
        drop table outbox_queue;
        drop table outbox_points;
        drop table student_courses;
        drop table submissions;
        drop table repositories;
        drop table google;
        drop table google_positions;
        drop table subject_course_groups;
        drop table mentors;
        drop table students;
        drop table users;
        drop table groups;
        drop table assignments;
        drop table courses;
        drop table subjects;
        """;
}