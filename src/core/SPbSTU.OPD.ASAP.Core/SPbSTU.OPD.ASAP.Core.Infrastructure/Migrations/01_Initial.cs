using FluentMigrator;
using SPbSTU.OPD.ASAP.Core.Infrastructure.Common;

namespace SPbSTU.OPD.ASAP.Core.Infrastructure.Migrations;

[Migration(1, "Initial migration")]
public class Initial : SqlMigration
{
    protected override string GetUpSql(IServiceProvider services) =>
        """
        create table submissions (
            id serial primary key,
            student_id bigint,
            assignment_id bigint,
            repository_id bigint,
            created_at timestamp with time zone not null,
            updated_at timestamp with time zone not null
        );

        create table assignments (
            id serial primary key,
            course_id bigint,
            title varchar not null,
            description varchar,
            created_at timestamp without time zone
        );

        create table subjects (
            id serial primary key,
            title varchar not null
        );

        create table courses (
            id serial primary key,
            title varchar not null,
            subject_id bigint,
            github_organization varchar not null
        );

        create table groups (
            id bigint primary key
        );

        create table students (
            id bigint primary key,
            name varchar not null,
            group_id bigint,
            user_id bigint
        );

        create table subject_course_groups (
            id serial primary key,
            course_id bigint,
            group_id bigint,
            mentor_id bigint
        );

        create table mentors (
            id serial primary key,
            name varchar not null,
            user_id bigint
        );

        create table users (
            id serial primary key,
            login varchar not null,
            password varchar not null,
            role varchar not null,
            github_link varchar not null
        );

        create table google (
            id serial primary key,
            student_id bigint,
            course_id bigint,
            assignment_id bigint,
            student_position varchar not null,
            assignment_position varchar not null
        );

        create table repositories (
            id serial primary key,
            student_id bigint,
            assignment_id bigint,
            link varchar not null
        );

        create table student_courses (
            id serial primary key,
            student_id bigint,
            course_id bigint,
            is_invited boolean not null
        );

        create table outbox_points (
            id serial primary key,
            points integer not null,
            date timestamp with time zone,
            course_id bigint,
            student_position varchar not null,
            assignment_position varchar not null,
            is_sent boolean not null
        );

        create table outbox_queue (
            id serial primary key,
            link varchar not null,
            mentor_id bigint,
            assignment_id bigint,
            submission_id bigint,
            is_sent boolean not null
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