﻿using Dapper;
using SPbSTU.OPD.ASAP.Core.Domain.Contracts.Repositories;
using SPbSTU.OPD.ASAP.Core.Domain.Models;

namespace SPbSTU.OPD.ASAP.Core.Persistence.Repositories;

public class StudentRepository(string connectionString) : PgRepository(connectionString), IStudentRepository
{
    public async Task<List<Student>> GetByCourseId(long courseId, CancellationToken ct)
    {
        const string sqlQuery = 
            """
            select s.id as id
                 , u.name as name
              from student_courses sc
              join users u on u.id = sc.user_id
              join students s on s.user_id = u.id
             where sc.course_id = @CourseId;
            """;
        
        await using var connection = await GetConnection();
        var students = await connection.QueryAsync<Student>(
            new CommandDefinition(
                sqlQuery,
                new { CourseId = courseId },
                cancellationToken: ct));
        return students.ToList();
    }
}