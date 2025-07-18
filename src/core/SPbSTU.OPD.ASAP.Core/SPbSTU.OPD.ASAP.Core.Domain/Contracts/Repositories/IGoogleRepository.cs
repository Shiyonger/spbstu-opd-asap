﻿using SPbSTU.OPD.ASAP.Core.Domain.Models;
using SPbSTU.OPD.ASAP.Core.Domain.ValueObjects;

namespace SPbSTU.OPD.ASAP.Core.Domain.Contracts.Repositories;

public interface IGoogleRepository
{
    Task<Dictionary<(string, long), Position>> GetStudentsPositions(List<(string, long)> githubUsernamesAndCourses,
        CancellationToken ct);

    Task<Dictionary<(string, long), Position>> GetAssignmentsPositions(List<(string, long)> titlesAndCourses,
        CancellationToken ct);

    Task CreatePositions(long courseId, List<Student> students, List<Assignment> assignments, CancellationToken ct);
}