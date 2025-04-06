using FluentMigrator;
using SPbSTU.OPD.ASAP.Core.Persistence.Common;

namespace SPbSTU.OPD.ASAP.Core.Persistence.Migrations;

[Migration(1, "Initial migration")]
public class Initial : SqlMigration
{
    protected override string GetUpSql(IServiceProvider services) =>
        """
        create table subjects (
            id serial primary key,
            title varchar not null
        );
        
        create table courses (
            id serial primary key,
            title varchar not null,
            subject_id bigint references subjects (id),
            github_organization varchar not null
        );
        
        create table assignments (
            id serial primary key,
            course_id bigint references courses (id),
            title varchar not null,
            description varchar,
            created_at timestamp without time zone
        );
        
        create table groups (
            id bigint primary key
        );
        
        create table users (
            id serial primary key,
            login varchar not null,
            password varchar not null,
            role varchar not null,
            github_link varchar not null
        );
        
        create table students (
            id bigint primary key,
            name varchar not null,
            group_id bigint references groups (id),
            user_id bigint references users (id)
        );
        
        create table mentors (
            id serial primary key,
            name varchar not null,
            user_id bigint references users (id)
        );
        
        create table subject_course_groups (
            id serial primary key,
            course_id bigint references courses (id),
            group_id bigint references groups (id),
            mentor_id bigint references mentors (id)
        );
        
        create table google (
            id serial primary key,
            student_id bigint references students (id),
            course_id bigint references courses (id),
            assignment_id bigint references assignments (id),
            student_position varchar not null,
            assignment_position varchar not null
        );
        
        create table repositories (
            id serial primary key,
            student_id bigint references students (id),
            assignment_id bigint references assignments (id),
            link varchar not null
        );

        create table submissions (
            id serial primary key,
            student_id bigint references students (id),
            assignment_id bigint references assignments (id),
            repository_id bigint references repositories (id),
            created_at timestamp with time zone not null,
            updated_at timestamp with time zone not null
        );

        create table student_courses (
            id serial primary key,
            student_id bigint references students (id),
            course_id bigint references courses (id),
            is_invited boolean not null
        );

        create table outbox_points (
            id serial primary key,
            points integer not null,
            date timestamp with time zone,
            course_id bigint references courses (id),
            student_position varchar not null,
            assignment_position varchar not null,
            is_sent boolean not null
        );

        create table outbox_queue (
            id serial primary key,
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
        drop table submissions;
        drop table assignments;
        drop table subjects;
        drop table courses;
        drop table groups;
        drop table students;
        drop table subject_course_groups;
        drop table mentors;
        drop table users;
        drop table google;
        drop table repositories;
        drop table student_courses;
        drop table outbox_points;
        drop table outbox_queue;
        """;
}